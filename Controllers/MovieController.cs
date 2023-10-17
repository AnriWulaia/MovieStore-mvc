using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Repositories.Abstract;

namespace MovieStoreMvc.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genreService;
        public MovieController(IMovieService movieService, IFileService fileService, IGenreService genreService)
        {
            _movieService = movieService;
            _fileService = fileService;
            _genreService = genreService;
        }
        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "Not Valid";

                // Output validation errors to the console or log them
                foreach (var key in ModelState.Keys)
                {
                    var modelStateEntry = ModelState[key];
                    foreach (var error in modelStateEntry.Errors)
                    {
                        // Log or output the validation error message
                        System.Diagnostics.Debug.WriteLine($"Validation Error for {key}: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }
            var fileResult = _fileService.SaveImage(model.ImageFile);
            if (fileResult.Item1 == 0)
            {
                TempData["msg"] = "File Coultn't be saved";
            }
            var imageName = fileResult.Item2;
            model.MovieImage = imageName;
            var result = _movieService.Add(model);
            if (result)
            {
                TempData["msg"] = "Successfully added";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Could not add";
                return View(model);
            }

        }


        public IActionResult Edit(int id)
        {
            var data = _movieService.GetById(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(Movie model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _movieService.Update(model);
            if (result)
            {
                return RedirectToAction(nameof(MovieList));
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult MovieList()
        {
            var data = _movieService.List();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            var result = _movieService.Delete(id);
            return RedirectToAction(nameof(MovieList));
        }
    }
}
