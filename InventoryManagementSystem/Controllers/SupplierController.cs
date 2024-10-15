using InventoryManagementSystem.BLL.Services;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _isupplierService;

        public SupplierController(ISupplierService isupplierService)
        {
            _isupplierService = isupplierService;
        }

        // Index action method to get the list of suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _isupplierService.GetAllAsync(); // Await the async method
            return View(suppliers);
        }

        // Get action for creating a new supplier
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Post action for creating a new supplier
        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _isupplierService.AddAsync(supplier); // Ensure this method is async
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        // Get action for supplier details
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var supplier = await _isupplierService.GetSupplierByIdAsync(id.Value); // Ensure async call
            if (supplier is null)
                return NotFound();

            return View(viewName, supplier);
        }

        // Get action for editing a supplier
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit"); // Use the details method to get the supplier
        }

        // Post action for editing a supplier
        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier, [FromRoute] int id)
        {
            if (id != supplier.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _isupplierService.UpdateAsync(supplier); // Ensure this method is async
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(supplier);
        }

        // Get action for deleting a supplier
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete"); // Use the details method to get the supplier
        }

        // Post action for deleting a supplier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _isupplierService.DeleteAsync(supplier); // Ensure this method is async
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(supplier);
        }
    }
}
