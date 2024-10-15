using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Pl.Controllers
{
    [Authorize(Roles = "User")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Action to list all users and their roles
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = await GetUserDtos(users);

            return View(userDtos);
        }

        // Action to manage roles for a specific user
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var rolesDto = await GetUserRolesDto(user);
            return View(rolesDto);
        }

        // POST action to update user roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            await UpdateUserRoles(user, model.Roles);

            return RedirectToAction(nameof(Index));
        }

        // Helper method to get user DTOs
        private async Task<List<UserDto>> GetUserDtos(IEnumerable<User> users)
        {
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id.ToString(),
                    Name = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return userDtos;
        }

        // Helper method to get user roles DTO
        private async Task<UserRolesDto> GetUserRolesDto(User user)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            return new UserRolesDto
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
        }

        // Helper method to update user roles
        private async Task UpdateUserRoles(User user, IEnumerable<RoleDto> roleDtos)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in roleDtos)
            {
                if (role.IsSelected && !currentRoles.Contains(role.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else if (!role.IsSelected && currentRoles.Contains(role.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }
        }
    }
}
