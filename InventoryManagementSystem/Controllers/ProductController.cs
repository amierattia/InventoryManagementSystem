using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using InventoryManagementSystem.Pl.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IInventoryService _inventoryService;
        private readonly ISupplierService _supplierService;
        private readonly IActivityService _activityService;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<StockHub> _hubContext;


        public ProductController(IHubContext<StockHub> hubContext, IInventoryService inventoryService, IProductService productService, ICategoryService categoryService, ISupplierService supplierService, IActivityService activityService, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _activityService = activityService;
            _userManager = userManager;
            _hubContext = hubContext;
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            await UpdateStockAsync();
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAndSuppliersToViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAndSuppliersToViewBag();
                return View(product);
            }

            if (!await IsValidCategory(product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "The selected category is invalid.");
                await LoadCategoriesAndSuppliersToViewBag();
                return View(product);
            }

            await _productService.AddAsync(product);
            await UpdateStockAsync();

            var userId = _userManager.GetUserId(User);
            var userName = _userManager.GetUserName(User);
            await _activityService.LogActivity($"Product {product.ProductName} created by {userName}.", userName);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            await LoadCategoriesAndSuppliersToViewBag();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid || product == null || !await IsProductIdValid(product.ProductId))
                return BadRequest();

            try
            {
                await _productService.UpdateAsync(product);
                await UpdateStockAsync();
                var userId = _userManager.GetUserId(User);
                var userName = _userManager.GetUserName(User);
                await _activityService.LogActivity($"Product {product.ProductName} edited by {userName}.", userName);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            await _productService.DeleteAsync(product);

            var userId = _userManager.GetUserId(User);
            var userName = _userManager.GetUserName(User);
            await _activityService.LogActivity($"Product {product.ProductName} deleted by {userName}.", userName);

            return RedirectToAction(nameof(Index));
        }

        



        // Private helper methods
        private async Task LoadCategoriesAndSuppliersToViewBag()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Suppliers = await _supplierService.GetAllAsync();
        }

        private async Task<bool> IsValidCategory(int categoryId)
        {
            var categories = await _categoryService.GetAllAsync();
            return categories.Any(c => c.CategoryId == categoryId);
        }

        private async Task<bool> IsProductIdValid(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            return product != null;
        }

        // stock 
        public async Task UpdateStockAsync()
        {
            var products = await _productService.GetAllAsync();

            foreach (var product in products)
            {
                if (product.stockQuantity < 5)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveLowStockNotification", product.ProductName, product.stockQuantity);
                }
            }
        }

    }
}
