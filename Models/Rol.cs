using System.ComponentModel.DataAnnotations;

namespace VideogamesPOS.Models
{
    public class Rol
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} must have less than {1} characters ")]
        public string Name { get; set; } // "Admin", "Cliente"
        public string Description { get; set; }

        public ICollection<User> Users { get; set; } // Relación inversa
    }

}
