using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SkladIdentity.Models;
using SkladIdentity.ViewModels;

namespace Sklad.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> user, SignInManager<User> sign)
        {
            _userManager = user;
            _signInManager = sign;
        }
        public IActionResult Login()
        {
            if (User.Identity is { IsAuthenticated: true })
                return RedirectToAction("Index", "Home");
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Wrong login or password");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Create()
        {
            var res = await _userManager.CreateAsync(new User() { UserName = "Admin" }, "Fb4a6a22_a");
            if (res.Succeeded)
            {

            }
            return RedirectToAction("Login");
        }

    }
}
