using InventoryManagementSystem.BLL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Pl.Controllers
{
 

    namespace InventoryManagementSystem.Pl.Controllers
    {
        public class UserRoleController : Controller
        {
            private readonly UserManager<User> _userManager; 
            private readonly RoleManager<IdentityRole> _roleManager;

            public UserRoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<IActionResult> Index()
            {
                var users = await _userManager.Users.ToListAsync();
                var userRoles = new List<UserRoleDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userRoles.Add(new UserRoleDto
                    {
                       // UserId = user.Id,
                        UserName = user.UserName,
                        Roles = roles.ToList()
                    });
                }

                return View(userRoles);
            }




            [HttpPost]
            public async Task<IActionResult> AddToRole(string userId, string roleName)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return BadRequest("User not found or role not assigned.");
            }

            [HttpPost]
            public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return BadRequest("User not found or role not removed.");
            }
        }
    }

}
