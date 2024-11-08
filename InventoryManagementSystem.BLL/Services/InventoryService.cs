﻿using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.sln.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationContext _context;

        public InventoryService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalItemsAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetLowStockItemsAsync()
        {
            var p = await _context.Products.CountAsync(p => p.stockQuantity < 5);
            return p;
        }
        public async Task<List<Product>> GetAllLowStockItemsAsync()
        {
            return await _context.Products
                .Where(p => p.stockQuantity < 5)
                .ToListAsync(); 
        }


        public async Task<int> GetUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetCategoryAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<List<ActivityModel>> GetRecentActivityAsync()
        {
            return await _context.Activities.OrderByDescending(a => a.Date).ToListAsync();
        }
    }
}
