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
    [Authorize(Roles = "Admin,Manager")]
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
           
                var users= await _userManager.Users.Select( user => new UserDto
                {
                    //  UserId = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Id = user.Id.ToString(),
                    Roles =  _userManager.GetRolesAsync(user).Result.ToList(),

                }).ToListAsync();
                
            
            return View(users);
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
                Roles = new List<RoleDto>() // Ensure this is RoleDto if that's what your model requires
            };

            foreach (var role in roles)
            {
                var isSelected = await _userManager.IsInRoleAsync(user, role.Name); // Check if user is in role
                oUserRolesDto.Roles.Add(new RoleDto // Ensure you are adding the correct type
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
