using AutoMapper;
using InventoryManagementSystem.BLL.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Pl.Controllers
{
    [Authorize(Roles = "User")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // Action to display all roles
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // GET action to display role creation form
        [HttpGet]
        public IActionResult Create() => View();

        // POST action to create a new role
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await CreateRoleAsync(model.Name);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            AddErrorsToModelState(result);
            return View(model);
        }

        // GET action to display edit form for a specific role
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            var model = new CreateRoleDto { Name = role.Name };
            return View(model);
        }

        // POST action to update an existing role
        [HttpPost]
        public async Task<IActionResult> Edit(string id, CreateRoleDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await UpdateRoleAsync(id, model.Name);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            AddErrorsToModelState(result);
            return View(model);
        }

        // GET action to display role deletion confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        // POST action to confirm and delete a role
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await DeleteRoleAsync(id);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            AddErrorsToModelState(result);
            return RedirectToAction(nameof(Index));
        }

        // Helper method to create a role
        private async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole { Name = roleName };
            return await _roleManager.CreateAsync(role);
        }

        // Helper method to get a role by Id
        private async Task<IdentityRole> GetRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        // Helper method to update a role
        private async Task<IdentityResult> UpdateRoleAsync(string id, string roleName)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            role.Name = roleName;
            return await _roleManager.UpdateAsync(role);
        }

        // Helper method to delete a role
        private async Task<IdentityResult> DeleteRoleAsync(string id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            return await _roleManager.DeleteAsync(role);
        }

        // Helper method to add errors to ModelState
        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
