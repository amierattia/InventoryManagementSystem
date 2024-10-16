using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.DAL.Db;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly FileService _fileService;
        private readonly IActivityService _activityService;
        private readonly UserManager<User> _userManager;

        public CategoryController(ICategoryService categoryService, FileService fileService, IActivityService activityService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _fileService = fileService;
            _activityService = activityService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories ?? new List<Category>());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            if (category.Photo != null)
            {
                string uniqueFileName = _fileService.UploadFile(category.Photo, "images");
                category.PhotoName = uniqueFileName;
            }

            await _categoryService.UpdateAsync(category);

            // Log activity
            var userName = _userManager.GetUserName(User);
            await _activityService.LogActivity($"Category {category.Name} edited by user {userName}.", userName);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            await _categoryService.DeleteAsync(category);

            // Log activity
            var userId = _userManager.GetUserName(User);
            await _activityService.LogActivity($"Category {category.Name} deleted by user {userId}.", userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            if (category.Photo != null)
            {
                string uniqueFileName = _fileService.UploadFile(category.Photo, "images");
                category.PhotoName = uniqueFileName;
            }

            await _categoryService.AddAsync(category);

            // Log activity
            var userId = _userManager.GetUserName(User);
            await _activityService.LogActivity($"Category {category.Name} created by user {userId}.", userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

    }
}
