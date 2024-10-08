using InventoryManagementSystem.BLL;
using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.BLL.sln.Dto;
using InventoryManagementSystem.BLL.sln.Services;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IUserService _customerService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationContext _context;

        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, UserManager<User> userManager, IUserService _customerService, ApplicationContext _context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this._context = _context;
            this.roleManager = roleManager;
            this._customerService = _customerService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Rgister model)
        {
           
            if (!ModelState.IsValid) return View(model);

            var User = new User
            {
                Name = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,
                RoleId = 1
            };

            var result = await _customerService.RegisterCustomerAsync(User, model.Password);
            if (result.Succeeded)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
                new Claim(ClaimTypes.Email, User.Email)
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("Cookies", claimsPrincipal, authProperties);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
            }

            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordDto { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
        }

        // تسجيل الخروج
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
