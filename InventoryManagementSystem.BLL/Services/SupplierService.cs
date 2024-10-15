using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationContext _context;

        public SupplierService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<int> AddAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            return await _context.SaveChangesAsync(); 
        }

        public async Task<int> DeleteAsync(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            return await _context.SaveChangesAsync(); 
        }

        public async Task<int> UpdateAsync(Supplier supplier) 
        {
            _context.Suppliers.Update(supplier);
            return await _context.SaveChangesAsync(); 
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id) 
        {
            return await _context.Suppliers.FindAsync(id);
        }
    }
}
