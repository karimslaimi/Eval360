using Eval360.Models;
using Eval360.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eval360.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("/login", Name = "Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.userName, model.password, true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    User user = await this.userManager.FindByNameAsync(model.userName);
                    // Redirect to the appropriate page after successful login
                    //todo redirect based on role
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we reach this point, the login attempt failed
            return View(model);
        }

        [HttpGet("/logout", Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
            var currentUser = User.Identity.Name;
            // Sign out the user
            await _signInManager.SignOutAsync();

            // Clear the existing authentication cookies
            await this._httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to a specific page after logout
            return RedirectToAction("Index", "Home");
        }


    }
}
