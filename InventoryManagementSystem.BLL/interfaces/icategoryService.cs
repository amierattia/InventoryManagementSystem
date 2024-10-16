using InventoryManagementSystem.EntitiesLayer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.BLL.interfaces
{
    public interface ICategoryService
    {
        Task AddAsync(Category entity);
        Task DeleteAsync(Category entity);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task UpdateAsync(Category entity);
        Task<Category> GetCategoryWithProductsAsync(int id);
        

    }
}
