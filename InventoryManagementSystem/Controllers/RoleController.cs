using AutoMapper;
using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Pl.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper Mapper;

        public RoleController(IMapper Mapper, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            this.Mapper = Mapper;
        }

        public IActionResult Index()
        {
            // get all roles 
            var roles = roleManager.Roles.ToList();
            // result
            //var result = Mapper.Map<List<RoleDto>>(roles);
            return View(roles);
        }

        // get role
        [HttpGet]
        public IActionResult Create() => View();

        // create role 
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.Name
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to create role.");
            }
            return View(model);
        }

        // get role by id for editing
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var model = new CreateRoleDto { Name = role.Name };
            return View(model);
        }

        // edit role
        [HttpPost]
        public async Task<IActionResult> Edit(string id, CreateRoleDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Failed to update role.");
                }
            }
            return View(model);
        }

        // delete role
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        // delete role confirmation
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to delete role.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
