﻿using InventoryManagementSystem.BLL.sln.Services;
using InventoryManagementSystem.BLL.sln.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryManagementSystem.DAL.Db;
using Microsoft.EntityFrameworkCore; 

namespace InventoryManagementSystem.PL.sln.Controllers
{
    [Authorize(Roles = "Admin")]
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

            var model = new DashboardDto
            {
                TotalItems = totalItems,
                LowStockItems = lowStockItems,  
                UsersCount = usersCount,
                RecentActivities = recentActivities
            };

            return View(model);
        }

    }
}
