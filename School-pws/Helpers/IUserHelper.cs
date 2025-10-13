using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;
using School_pws.Models.Users;
using System.Security.Claims;

namespace School_pws.Helpers
{
    public interface IUserHelper
    {
        Task<List<User>> GetAllAsync(ClaimsPrincipal currentUser);

        Task<User> GetUserById(string id);

        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password, bool isVerified);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> RemovePasswordAsync(User user);

        Task<IdentityResult> AddPasswordAsync(User user, string password);

        Task<IdentityResult> DeleteUserAsync(string id);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<bool> SendEmailToRecoryPassword(User user);

        Task CheckRoleAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        IEnumerable<SelectListItem> GetComboRoles();

        Task<bool> HasDependenciesAsync(string id);
    }
}
