
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationContext _context;

        public CategoryService(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(Category entity)
        {
            _context.Add(entity);
            _context.SaveChanges(); 
        }

        public void delete(Category entity)
        {
            _context.Categories.Remove(entity);
            _context.SaveChanges();
        }

        public Category? GetById(int? id)
        {
            return _context.Find<Category>(id);

        }

        public async Task<IEnumerable<Category>> GetAll()
        {

            return await _context.Set<Category>().AsNoTracking().ToListAsync();
        }

        public void Update(Category entity)
        {
            _context.Set<Category>().Update(entity);
            _context.SaveChanges();
        }
        public int Save()
        {
            return _context.SaveChanges();
        }

    }
}

