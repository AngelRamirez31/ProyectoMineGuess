using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVP_ProyectoFinal.Models;

namespace MVP_ProyectoFinal.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel
            {
                UserName = string.Empty,
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl)
        {
            userName = userName == null ? string.Empty : userName.Trim();
            password = password == null ? string.Empty : password.Trim();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["LoginError"] = "User name and password are required.";
                var invalidModel = new LoginViewModel
                {
                    UserName = userName,
                    ReturnUrl = returnUrl
                };
                return View(invalidModel);
            }

            var user = UserRepository.ValidateUser(userName, password);
            if (user == null)
            {
                ViewData["LoginError"] = "Invalid user name or password.";
                var invalidModel = new LoginViewModel
                {
                    UserName = userName,
                    ReturnUrl = returnUrl
                };
                return View(invalidModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null)
            {
                model = new RegisterViewModel();
            }

            var userName = model.UserName == null ? string.Empty : model.UserName.Trim();
            var password = model.Password == null ? string.Empty : model.Password.Trim();
            var confirmPassword = model.ConfirmPassword == null ? string.Empty : model.ConfirmPassword.Trim();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return View(model);
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(model);
            }

            if (UserRepository.UserExists(userName))
            {
                ModelState.AddModelError(string.Empty, "User name is already taken.");
                return View(model);
            }

            var user = UserRepository.CreateUser(userName, password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
