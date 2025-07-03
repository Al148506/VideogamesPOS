using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VideogamesPOS.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
