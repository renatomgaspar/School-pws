using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_pws.Data;
using School_pws.Data.Entities;
using School_pws.Helpers;
using School_pws.Models.Users;

namespace School_pws.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IEncryptHelper _encryptHelper;

        public AccountController(
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IEncryptHelper encryptHelper
            )
        {
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _encryptHelper = encryptHelper;
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Manage()
        {
            return View(await _userHelper.GetAllAsync(User));
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Roles = _userHelper.GetComboRoles()
                    .Where(r => r.Value != "Student")
                    .ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (!User.IsInRole("Admin"))
            {
                model.Role = "Student";
            }

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)

                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    user = _converterHelper.ToUser(model, imageId, true);

                    var result = await _userHelper.AddUserAsync(user, "", false);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.CheckRoleAsync(model.Role);
                    await _userHelper.AddUserToRoleAsync(user, model.Role);

                    TempData["SuccessMessage"] = "User created successfully!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There is a user with that email already!");
                }   
            }

            model.Roles = _userHelper.GetComboRoles();
            return View(model);
        }

        public IActionResult Activate(string id)
        {
            var model = new ActiveAccountViewModel
            {
                Id = id
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Activate(ActiveAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _encryptHelper.DecryptString(model.Id);

                var user = await _userHelper.GetUserById(model.Id);
                if (user != null)

                {
                    if (user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "User is already verified!");
                        return View(model);
                    }

                    var passwordResult = await _userHelper.AddPasswordAsync(user, model.NewPassword);

                    if (passwordResult.Succeeded)
                    {
                        user.EmailConfirmed = true;
                        var result = await _userHelper.UpdateUserAsync(user);

                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "User is Activated!";
                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Was not possible to activate your account!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Could not set the new password!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found!");
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
                else if (result.IsNotAllowed)
                {
                    this.ModelState.AddModelError(string.Empty, "You must verify your account first");
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("TwoFactorLogin");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to Login");
            return View(model);
        }

        public IActionResult TwoFactorLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactorLogin(string code)
        {
            var result = await _userHelper.TwoFactorLoginAsync(code);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid Code");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.TwoFactor = user.TwoFactorEnabled;
                model.ImageId = user.ImageId;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        await _blobHelper.DeleteBlobAsync(user.ImageId, "users");
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                        user.ImageId = imageId;
                    }

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.TwoFactorEnabled = model.TwoFactor;
                    model.ImageId = user.ImageId;
                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }

        public IActionResult RecoveryPassword(string id)
        {
            var model = new ActiveAccountViewModel
            {
                Id = id
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoveryPassword(ActiveAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _encryptHelper.DecryptString(model.Id);

                var user = await _userHelper.GetUserById(model.Id);
                if (user != null)

                {
                    var removePasswordResult = await _userHelper.RemovePasswordAsync(user);

                    if (removePasswordResult.Succeeded)
                    {
                        var passwordResult = await _userHelper.AddPasswordAsync(user, model.NewPassword);

                        if (passwordResult.Succeeded)
                        {
                            var result = await _userHelper.UpdateUserAsync(user);

                            if (result.Succeeded)
                            {
                                TempData["SuccessMessage"] = "Password was changed! Login with your new password";
                                return View(model);
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Was not possible to change the password from your account!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Could not set the new password!");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found!");
                }
            }

            return View(model);
        }

        public IActionResult RecoveryPasswordEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoveryPasswordEmail(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user != null)

                {
                    var result = await _userHelper.SendEmailToRecoryPassword(user);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "The email to set the new password was sent.";
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Something went wrong.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found!");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userHelper.GetUserById(id);

            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "User not found.");
            }

            return View(user);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userHelper.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userHelper.GetUserById(user.Id);
                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.UserName = user.Email;

                    var result = await _userHelper.UpdateUserAsync(existingUser);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Manage));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return View(user);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userHelper.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            if (await _userHelper.HasDependenciesAsync(id))
            {
                ViewBag.ErrorTitle = $"{user.UserName} can not be deleted!";
                ViewBag.ErrorMessage = $"The User have applications or have subjects created in is name!";
                return View("Error");
            }

            await _userHelper.DeleteUserAsync(id);
            return RedirectToAction(nameof(Manage));
        }


        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
