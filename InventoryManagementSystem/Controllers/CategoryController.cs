using InventoryManagementSystem.BLL.Services;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryManagementSystem.Pl.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _icategoryService;
        private readonly FileService _fileService;

        public CategoryController(ICategoryService categoryService, FileService fileService)
        {
            _icategoryService = categoryService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _icategoryService.GetAll();
            return View(categories ?? new List<Category>());
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var category = _icategoryService.GetById(Id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category Cat)
        {
            if (ModelState.IsValid)
            {
                if (Cat.Photo != null)
                {
                    string UniqFile = _fileService.UploadFile(Cat.Photo, "images");
                    Cat.PhotoName = UniqFile;
                }
                _icategoryService.Update(Cat);
                return RedirectToAction(nameof(Index));
            }
            return View(Cat);
        }


        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var FId =  _icategoryService.GetById(Id);
            if(FId == null)
            {
                return NotFound();
            }
            else
            {
                return View(FId);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirme(int id) 
        {
            var category = _icategoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            _icategoryService.delete(category);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Photo != null)
                {
                    string UniqFile = _fileService.UploadFile(category.Photo, "images");
                    category.PhotoName = UniqFile;
                }

                _icategoryService.Add(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }



    }



}
