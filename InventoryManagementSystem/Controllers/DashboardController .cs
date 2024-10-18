using InventoryManagementSystem.BLL.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.BLL.interfaces;

namespace InventoryManagementSystem.PL.sln.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class DashboardController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public DashboardController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var totalItems = await _inventoryService.GetTotalItemsAsync();
            var lowStockItems = await _inventoryService.GetLowStockItemsAsync();
            var usersCount = await _inventoryService.GetUsersCountAsync();
            var recentActivities = await _inventoryService.GetRecentActivityAsync();
            var CategoryCount = await _inventoryService.GetCategoryAsync();

            var model = new DashboardDto
            {
                TotalItems = totalItems,
                LowStockItems = lowStockItems,  
                UsersCount = usersCount,
                RecentActivities = recentActivities,
                CategoryCount = CategoryCount
            };

            return View(model);
        }

    }
}
