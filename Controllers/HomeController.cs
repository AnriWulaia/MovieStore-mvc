using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Abstract;
using MovieStoreMvc.Repositories.Implementation;
using Newtonsoft.Json.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MovieStoreMvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMovieService _movieService;
        private readonly IConfiguration _configuration;
        private readonly string apiKey;

        private readonly IMemoryCache _cache;
        public HomeController(IMovieService movieService, IMemoryCache cache, IConfiguration configuration)
        {
            _movieService = movieService;
            _cache = cache;
            _configuration = configuration;

            apiKey = _configuration["ApiKeys:MyApiKey"];
        }
        [Route("")]
        [Route("Home")]
        public IActionResult Index(int currentPage = 1, string term = "")
        {
            var movies = _movieService.List(term, true,currentPage);
            return View(movies);
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> MovieDetails(int id)
        {
            var movie = _movieService.GetById(id);
            string keyword = $"{movie.Title} {movie.ReleaseYear} Trailer";
            string videoId = await SearchYouTubeVideoAsync(keyword, apiKey);
            string embeddedVideo = $"https://www.youtube.com/embed/{videoId}?vq=hd1080&rel=0&iv_load_policy=3";
            movie.VideoUrl = embeddedVideo;
            movie.GenreNames = _movieService.GenreList(id);
            
            return View(movie);
        }

        private async Task<string> SearchYouTubeVideoAsync(string query, string apiKey)
        {
            // check if the data is already in cache
            if (_cache.TryGetValue("VideoData_" + query, out string videoId))
            {
                return videoId;
            }

            string apiUrl = $"https://www.googleapis.com/youtube/v3/search?q={query}&key={apiKey}&part=snippet&type=video&maxResults=1";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(apiUrl);
                    var jsonResponse = JObject.Parse(response);
                    videoId = jsonResponse["items"][0]["id"]["videoId"].ToString();

                    // cache for 1 hour
                    _cache.Set("VideoData_" + query, videoId, TimeSpan.FromDays(1));

                    return videoId;
                }
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }



    }
}
