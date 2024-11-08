﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.BLL.Services;

namespace UsersApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IProfileService _profileService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IActivityService _activityService;
        private readonly FileService _fileService;

        public AccountController(
            IActivityService activityService,
            IProfileService profileService,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            FileService fileService,
            RoleManager<IdentityRole> roleManager)
        {
            _activityService = activityService;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _profileService = profileService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return NotFound();

            var model = new EditProfileDto
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileDto model)
        {
            if (!ModelState.IsValid)  ModelState.AddModelError(string.Empty, "invalid data !");

            var userId = _userManager.GetUserId(User);
            var result = await _profileService.UpdateProfileAsync(userId, model);

            if (result == UpdateProfileResult.UserNotFound)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(model);
            }

            if (result == UpdateProfileResult.EmailExists)
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return View(model);
            }

            return RedirectToAction(nameof(UserProfile), new { id = userId });
        }


        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var userId = _userManager.GetUserId(User);
                await _activityService.LogActivity($"User {model.Email} logged in.", userId);
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Email or password is incorrect.");
            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await IsEmailRegistered(model.Email))
            {
                ModelState.AddModelError("", "This email is already registered.");
                return View(model);
            }
            string? image = _fileService.UploadFile(model.ProfilePicture, "images");
            var newUser = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,
                ProfilePictureUrl = image
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                return RedirectToAction(nameof(Login));
            }

            AddErrorsToModelState(result);
            return View(model);
        }

        public IActionResult VerifyEmail() => View();

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View(model);
            }

            return RedirectToAction(nameof(ChangePassword), new { username = user.UserName });
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username)) return RedirectToAction(nameof(VerifyEmail));
            return View(new ChangePasswordDto { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found!");
                return View(model);
            }

            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
            {
                AddErrorsToModelState(result);
                return View(model);
            }

            result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (result.Succeeded) return RedirectToAction(nameof(Login));

            AddErrorsToModelState(result);
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

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
                return NotFound();
            }

            var userDto = new UserDto
            {
                Name = user.FullName ?? "Name not available",
                Email = user.Email ?? "Email not available",
                Phone = user.PhoneNumber ?? "Phone not available",
                ProfilePicture = user.ProfilePictureUrl,
            };

            return View(userDto);
        }
        private async Task<User> GetCurrentUserAsync() => await _userManager.GetUserAsync(User);

        private async Task<bool> IsEmailRegistered(string email) => await _userManager.FindByEmailAsync(email) != null;

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
