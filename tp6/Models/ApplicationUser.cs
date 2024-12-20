using Microsoft.AspNetCore.Identity;

namespace tp6.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Age { get; set; }
    }
}
