using Microsoft.AspNetCore.Razor.Hosting;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Repositories.Abstract;
using System;
using System.Security.Principal;

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
                var mg = ctx.MovieGenre.Where(a => a.MovieId == data.Id);
                foreach (var item in mg)
                {
                    ctx.MovieGenre.Remove(item);
                }
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
            foreach (var movie in list)
            {
                var genres = (from genre in ctx.Genre
                              join mg in ctx.MovieGenre
                              on genre.Id equals mg.GendreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join("\n", genres);
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
                var existingMovieGenres = ctx.MovieGenre.Where(a => a.MovieId == model.Id).ToList();
                ctx.MovieGenre.RemoveRange(existingMovieGenres);

                // Add the new selected genre associations
                foreach (int genId in model.Genres)
                {
                    var newMovieGenre = new MovieGenre { GendreId = genId, MovieId = model.Id };
                    ctx.MovieGenre.Add(newMovieGenre);
                }
                ctx.Movie.Update(model);
                

                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<int> GetGenreByMovieId(int movieId)
        {
            var genreIds = ctx.MovieGenre.Where(a => a.MovieId == movieId).Select(a => a.GendreId).ToList();
            return genreIds;
        }
        public string GetImageNameByMovieId(int movieId)
        {
            var imageName = ctx.Movie.Where(a => a.Id == movieId).Select(a => a.MovieImage).FirstOrDefault();
            return imageName;
        }
    }
}
