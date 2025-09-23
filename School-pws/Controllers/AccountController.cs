using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_pws.Helpers;
using School_pws.Models.Users;

namespace School_pws.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper
            )
        {
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
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

                    user = _converterHelper.ToStudent(model, imageId, true);

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    TempData["SuccessMessage"] = "User created successfully!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "There is a user with that email already!");
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
            }

            this.ModelState.AddModelError(string.Empty, "Failed to Login");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
