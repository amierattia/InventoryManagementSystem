using InventoryManagementSystem.BLL.Services;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _iproductService;


        public ProductController(IProductService iproductService)
        {
            _iproductService = iproductService;

        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var product = _iproductService.GetAll().Where(p => p.CategoryId == id);
            return View(product);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var product = new Product { CategoryId = id };
            return View(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _iproductService.Add(product);
                return RedirectToAction(nameof(Index), new { id = product.CategoryId });
            }

            return View(product);
        }


        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var product = _iproductService.GetProductById(id.Value);
            if (product is null)
                return NotFound();
            return View(ViewName, product);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]

        public IActionResult Edit(Product product, [FromRoute] int id)
        {
            if (id != product.ProductId)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _iproductService.Update(product);
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
        public IActionResult Delete([FromRoute] int? id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _iproductService.Delete(product);
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
