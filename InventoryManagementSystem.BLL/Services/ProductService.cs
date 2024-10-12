using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public int Add(Product product)
        {
            _context.Products.Add(product);
            return _context.SaveChanges();
        }

        public int Delete(Product product)
        {
            _context.Products.Remove(product);
            return _context.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Include(p => p.Supplier).ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Find<Product>(id);
        }

        public int Update(Product product)
        {
            _context.Update(product);
            return _context.SaveChanges();
        }
    }
}
