using System.ComponentModel.DataAnnotations;

namespace VideogamesPOS.Models
{
    public class SaleDetails
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public int SaleId { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public Sale Sale { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public int VideogameId { get; set; }
        public Videogame Videogame { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public decimal Price { get; set; }
    }

}
