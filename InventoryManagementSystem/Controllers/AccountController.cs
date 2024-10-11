using Humanizer;
using InventoryManagementSystem.BLL.Dto;
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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        public AccountController(
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserService userService,
            ApplicationContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("", "Invalid user ID.");
                return View();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            var userDto = new UserDto
            {
                Name = user.Name ?? "Name not available",
                Email = user.Email ?? "Email not available",
                Phone = user.PhoneNumber
            };

            return View(userDto);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Email or password is incorrect.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole("Admin");
                await _roleManager.CreateAsync(role);
            }

            var user = new User
            {
                Name = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,
                RoleId = 1
            };

            var result = await _userService.RegisterCustomerAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInUserAsync(user);
                return RedirectToAction("Login", "Account");
            }

            AddErrors(result);
            return View(model);
        }

        public IActionResult VerifyEmail() => View();

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View(model);
            }

            return RedirectToAction("ChangePassword", new { username = user.UserName });
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("VerifyEmail");

            return View(new ChangePasswordDto { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (removePasswordResult.Succeeded)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (addPasswordResult.Succeeded)
                    return RedirectToAction("Login");

                AddErrors(addPasswordResult);
            }
            else
            {
                AddErrors(removePasswordResult);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        private async Task SignInUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync("Cookies", claimsPrincipal, authProperties);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
