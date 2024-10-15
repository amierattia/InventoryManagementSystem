using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _context;

        public ProductService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return await SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Product product)
        {

            _context.Products.Remove(product); 
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await FindProductByIdAsync(productId);
        }

        public async Task<int> UpdateAsync(Product product)
        {
            var existingProduct = await FindProductByIdAsync(product.ProductId);
            ValidateProductExistence(existingProduct, product.ProductId);

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            return await SaveChangesAsync();
        }

        private async Task<Product> FindProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        private void ValidateProductExistence(Product product, int productId)
        {
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
        }

        private async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
