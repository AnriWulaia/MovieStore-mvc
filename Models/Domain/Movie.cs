using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? ReleaseYear { get; set; }
        [Required]
        public string? MovieImage { get; set; } // Movie Image Name with extension
        [Required]
        public string? Cast { get; set; }
        [Required]
        public string? Director { get; set; }
    }
}
