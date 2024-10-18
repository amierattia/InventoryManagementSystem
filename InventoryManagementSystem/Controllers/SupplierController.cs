using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IActivityService _activityService;
        private readonly UserManager<User> _userManager;

        public SupplierController(ISupplierService supplierService, IActivityService activityService, UserManager<User> userManager)
        {
            _supplierService = supplierService;
            _activityService = activityService;
            _userManager = userManager;
        }

        // GET: Supplier
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return View(suppliers);
        }

        // GET: Supplier/Create
        [HttpGet]
        public IActionResult Create() => View();

        // POST: Supplier/Create
        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            await _supplierService.AddAsync(supplier);

            // Logging activity
            var userName = _userManager.GetUserName(User);
            await _activityService.LogActivity($"Supplier {supplier.Name} was created by {userName}.", userName);

            return RedirectToAction(nameof(Index));
        }

        // GET: Supplier/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var supplier = await _supplierService.GetSupplierByIdAsync(id.Value);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // GET: Supplier/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id) => await GetSupplierView(id, "Edit");

        // POST: Supplier/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (!ModelState.IsValid || supplier == null)
                return View(supplier);

            try
            {
                await _supplierService.UpdateAsync(supplier);

                // Logging activity
                var userName = _userManager.GetUserName(User);
                await _activityService.LogActivity($"Supplier {supplier.Name} was edited by {userName}.", userName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(supplier);
            }
        }

        // GET: Supplier/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id) => await GetSupplierView(id, "Delete");

        // POST: Supplier/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Supplier supplier)
        {
            if (!ModelState.IsValid || supplier == null)
                return View(supplier);

            try
            {
                await _supplierService.DeleteAsync(supplier);

                // Logging activity
                var userName = _userManager.GetUserName(User);
                await _activityService.LogActivity($"Supplier {supplier.Name} was deleted by {userName}.", userName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(supplier);
            }
        }

        // Private helper method to get supplier details for Edit/Delete views
        private async Task<IActionResult> GetSupplierView(int? id, string viewName)
        {
            if (!id.HasValue)
                return BadRequest();

            var supplier = await _supplierService.GetSupplierByIdAsync(id.Value);
            if (supplier == null)
                return NotFound();

            return View(viewName, supplier);
        }
    }
}
