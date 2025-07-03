using System.ComponentModel.DataAnnotations;

namespace VideogamesPOS.Models.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
