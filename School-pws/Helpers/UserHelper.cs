using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_pws.Data;
using School_pws.Data.Entities;
using School_pws.Models.Users;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace School_pws.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEncryptHelper _encryptHelper;

        public UserHelper(
            DataContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEncryptHelper encryptHelper)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _encryptHelper = encryptHelper;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password, bool isVerified)
        {
            MailMessage email = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            email.From = new MailAddress("schoolmanagerpws@gmail.com");
            email.To.Add(user.Email);

            email.Subject = "Account Activation";

            email.IsBodyHtml = true;
            email.Body = $"Click to make your account Active <a href='https://localhost:44340/Account/Activate/?id={_encryptHelper.EncryptString(user.Id)}'>> HERE <</a>";

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("schoolmanagerpws@gmail.com", "lzqf lrqa jywi agkj");
            smtp.EnableSsl = true;
            smtp.Send(email);

            if (isVerified)
            {
                user.EmailConfirmed = true;
                return await _userManager.CreateAsync(user, password);
            }
            else
            {
                user.EmailConfirmed = false;
                return await _userManager.CreateAsync(user);
            } 
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

        public async Task<IdentityResult> AddPasswordAsync(User user, string password)
        {
            return await _userManager.AddPasswordAsync(user, password);
        }

        public async Task<IdentityResult> RemovePasswordAsync(User user)
        {
            return await _userManager.RemovePasswordAsync(user);
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

        public async Task<bool> SendEmailToRecoryPassword(User user)
        {
            MailMessage email = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            email.From = new MailAddress("schoolmanagerpws@gmail.com");
            email.To.Add(user.Email);

            email.Subject = "Recovery Password";

            email.IsBodyHtml = true;
            email.Body = $"Click to change your old password and recover your account <a href='https://localhost:44340/Account/RecoveryPassword/?id={_encryptHelper.EncryptString(user.Id)}'>> HERE <</a>";

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("schoolmanagerpws@gmail.com", "lzqf lrqa jywi agkj");
            smtp.EnableSsl = true;
            smtp.Send(email);

            return true;
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

        public async Task<bool> HasDependenciesAsync(string id)
        {
            return await _context.Subjects.AnyAsync(s => s.User.Id == id)
                || await _context.Applications.AnyAsync(a => a.User.Id == id) 
                || await _context.ApplicationDetailsTemp.AnyAsync(adt => adt.User.Id == id);
        }
    }
}
