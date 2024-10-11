using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Pl.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = users.Select(async user => new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.UserName,
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            }).Select(task => task.Result).ToList();

            return View(userDtos);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var userRolesDto = new UserRolesDto
            {
                UserName = user.UserName,
                UserId = user.Id.ToString(),
                Roles = roles.Select(role => new RoleDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                }).ToList()
            };

            return View(userRolesDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (role.IsSelected && !currentRoles.Contains(role.RoleName))
                    await _userManager.AddToRoleAsync(user, role.RoleName);

                if (!role.IsSelected && currentRoles.Contains(role.RoleName))
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
