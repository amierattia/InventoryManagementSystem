using InventoryManagementSystem.BLL.Services;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _iproductService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly ApplicationContext _context;

        public ProductController(ApplicationContext context, IProductService iproductService, ICategoryService categoryService, ISupplierService supplierService)
        {
            _iproductService = iproductService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var products = await _iproductService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAll();
            var suppliers = await _supplierService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Suppliers = suppliers;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Categories.Any(c => c.CategoryId == product.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", "The selected category is invalid.");
                    ViewBag.Categories = await _categoryService.GetAll();
                    return View(product);
                }

                await _iproductService.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryService.GetAll();
            return View(product);
        }




        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var product = _iproductService.GetProductByIdAsync(id.Value);
            if (product == null)
                return NotFound();

            return View(viewName, product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _iproductService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _categoryService.GetAll();
            ViewBag.Suppliers = await _supplierService.GetAllAsync(); 

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, [FromRoute] int id)
        {
            if (id != product.ProductId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _iproductService.UpdateAsync(product);
                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction(nameof(Index), new { id = product.CategoryId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _iproductService.DeleteAsync(product);
                    TempData["SuccessMessage"] = "Product deleted successfully!";
                    return RedirectToAction(nameof(Index), new { id = product.CategoryId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(product);
        }
    }
}
