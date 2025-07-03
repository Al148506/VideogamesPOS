using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideogamesPOS.Models
{
    public class Sale
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public DateTime SaleDate { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string PaymentMethod { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string Status { get; set; }

        public List<SaleDetails> SaleDetails { get; set; }
    }
}
