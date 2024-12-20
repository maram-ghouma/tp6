using Microsoft.AspNetCore.Identity;
using tp6.Models;

namespace tp6.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetUsersList()
        {
            return _userManager.Users;
        }
    }
}
