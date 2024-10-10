using InventoryManagementSystem.BLL;
using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InventoryManagementSystem.Pl.Controllers
{
    [Authorize( Roles = "Admin,User")]

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
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id=user.Id.ToString(),
                    Name = user.UserName,   
                    Email = user.Email,     
                    Roles = roles.ToList() 
                });
            }

            return View(userDtos);
        }


        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var oUserRolesDto = new UserRolesDto
            {
                UserName = user.UserName,
                UserId = user.Id.ToString(),
                Roles = new List<RoleDto>() 
            };

            foreach (var role in roles)
            {
                var isSelected = await _userManager.IsInRoleAsync(user, role.Name);
                oUserRolesDto.Roles.Add(new RoleDto 
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = isSelected
                });
            }

            return View(oUserRolesDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
