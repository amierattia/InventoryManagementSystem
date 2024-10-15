using InventoryManagementSystem.EntitiesLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(); 
        Task<Product> GetProductByIdAsync(int id);
        Task<int> DeleteAsync(Product product);
        Task<int> AddAsync(Product product);
        Task<int> UpdateAsync(Product product);
    }
}
