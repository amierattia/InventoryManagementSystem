using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.BLL.sln.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Pl.Controllers
{
    public class StockController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public StockController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index() => View(await _inventoryService.GetAllLowStockItemsAsync());



    }
}
