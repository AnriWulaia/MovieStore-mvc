using Microsoft.AspNetCore.Razor.Hosting;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Abstract;

namespace MovieStoreMvc.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DatabaseContext ctx;

        public MovieService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }

        public bool Add(Movie model)
        {
            try
            {
                ctx.Movie.Add(model);
                ctx.SaveChanges();
                foreach (int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GendreId = genreId
                    };
                    ctx.MovieGenre.Add(movieGenre);
                }
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = GetById(id);
                if (data == null)
                    return false;
                ctx.Movie.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
            return ctx.Movie.Find(id);
        }

        public MovieListVm List()
        {
            var list = ctx.Movie.ToList();
            foreach(var movie in list)
            {
                var genres = (from genre in ctx.Genre
                              join mg in ctx.MovieGenre
                              on genre.Id equals mg.GendreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }
            var data = new MovieListVm
            {
                MovieList = list.AsQueryable()
            };
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                ctx.Movie.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
