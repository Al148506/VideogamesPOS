﻿using System.ComponentModel.DataAnnotations;

namespace VideogamesPOS.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} must have less than {1} characters ")]
        public string Name { get; set; }
        public ICollection<Videogame> Videogames { get; set; }
    }
}
