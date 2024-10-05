using InventoryManagementSystem.BLL.sln.Services;
using InventoryManagementSystem.BLL.sln.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.PL.sln.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public DashboardController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var totalItems = await _inventoryService.GetTotalItemsAsync();
            var lowStockItems = await _inventoryService.GetLowStockItemsAsync();
            var usersCount = await _inventoryService.GetUsersCountAsync();
            var recentActivity = await _inventoryService.GetRecentActivityAsync();

            var model = new DashboardDto
            {
                TotalItems = totalItems,
                LowStockItems = lowStockItems,
                UsersCount = usersCount,
                RecentActivities = recentActivity
            };

            return View(model);
        }
    }
}
