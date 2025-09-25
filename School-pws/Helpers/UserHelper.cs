using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;
using School_pws.Models.Users;
using System.Security.Claims;

namespace School_pws.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<List<User>> GetAllAsync(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);

            if (user == null)
            {
                return new List<User>();
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var roles = new List<string> { "Admin", "Employee" };
                var users = new List<User>();

                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                    users.AddRange(usersInRole);
                }

                return users.Distinct()
                    .OrderBy(u => u.FullName)
                    .ToList(); 
            }

            return (await _userManager.GetUsersInRoleAsync("Student"))
                .OrderBy(u => u.FullName)
                .ToList();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser.Email != user.Email)
            {
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    existingUser.Email = user.Email;
                    existingUser.UserName = user.Email;
                }
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            return await _userManager.UpdateAsync(existingUser);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user,
            string oldPassword,
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .OrderBy(r => r.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a role...)",
                Value = string.Empty
            });

            return list;
        }
    }
}
