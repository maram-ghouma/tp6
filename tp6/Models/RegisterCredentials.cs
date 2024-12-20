using System.ComponentModel.DataAnnotations;

namespace tp6.Models
{
    public class RegisterCredentials
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }
      
        [Required]
        public string Email { get; set; }
    }
}
