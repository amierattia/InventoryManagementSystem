using InventoryManagementSystem.BLL.Services;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _isupplierService;
        public SupplierController(ISupplierService isupplierService)
        {
            _isupplierService = isupplierService;
        }

        public IActionResult Index()
        {
            var suppliers = _isupplierService.GetAll();
            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _isupplierService.Add(supplier);
                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        [HttpGet]

        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var supplier = _isupplierService.GetSupplierById(id.Value);
            if (supplier is null)
                return NotFound();
            return View(ViewName, supplier);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]

        public IActionResult Edit(Supplier supplier, [FromRoute] int id)
        {
            if (id != supplier.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _isupplierService.Update(supplier);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(supplier);
        }


        [HttpPost]

        public IActionResult Delete([FromRoute] int? id, Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _isupplierService.Delete(supplier);
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
