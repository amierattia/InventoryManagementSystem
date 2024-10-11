using InventoryManagementSystem.BLL.sln.Services;
using InventoryManagementSystem.BLL.sln.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.PL.sln.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class DashboardController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public DashboardController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var totalItemsTask = _inventoryService.GetTotalItemsAsync();
            var lowStockItemsTask = _inventoryService.GetLowStockItemsAsync();
            var usersCountTask = _inventoryService.GetUsersCountAsync();
            var recentActivityTask = _inventoryService.GetRecentActivityAsync();

            await Task.WhenAll(totalItemsTask, lowStockItemsTask, usersCountTask, recentActivityTask);

            var model = new DashboardDto
            {
                TotalItems = await totalItemsTask,
                LowStockItems = await lowStockItemsTask,
                UsersCount = await usersCountTask,
                RecentActivities = await recentActivityTask
            };

            return View(model);
        }
    }
}
