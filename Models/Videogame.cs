using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace VideogamesPOS.Models
{
    public class Videogame
    {
        public int Id { get; set; } // Unique identifier for the videogame
        [Required(ErrorMessage = "The field {0} is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The field {0} must be greater than 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than 0")]
        public int Stock { get; set; }
        public string ImageUrl { get; set; } // URL to the game's image
        [Required(ErrorMessage = "The field {0} is required")]
        public string Rating { get; set; } // e.g., "E", "T", "M" for ratings like Everyone, Teen, Mature
        [ValidateNever]
        public ICollection<Platform> Platforms { get; set; }
        [ValidateNever]
        public ICollection<Genre> Genres { get; set; }

    }
}
