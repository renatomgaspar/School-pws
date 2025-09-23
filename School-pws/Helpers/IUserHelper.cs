using Microsoft.AspNetCore.Identity;
using School_pws.Data.Entities;
using School_pws.Models.Users;

namespace School_pws.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
