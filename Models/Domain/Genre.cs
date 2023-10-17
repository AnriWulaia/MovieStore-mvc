using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models.Domain
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Genre Name")]
        public string? GenreName { get; set; }
    }
}
