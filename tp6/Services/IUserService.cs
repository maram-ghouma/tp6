using tp6.Models;

namespace tp6.Services
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetUsersList();
    }
}
