using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStoreMvc.Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Release Year")]
        public string? ReleaseYear { get; set; }

        public string? MovieImage { get; set; } // Movie Image Name with extension
        [Required]
        public string? Cast { get; set; }
        [Required]
        public string? Director { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [Required]
        public List<int>? Genres { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? GenreList { get; set; }
        [NotMapped]
        public string? GenreNames { get; set; }

        [NotMapped]
        public MultiSelectList? MultiGenreList { get; set; }

    }
}
