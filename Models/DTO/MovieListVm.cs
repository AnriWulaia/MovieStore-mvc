using MovieStoreMvc.Models.Domain;

namespace MovieStoreMvc.Models.DTO
{
    public class MovieListVm
    {
        public IQueryable<Movie> MovieList { get; set; }
    }
}
