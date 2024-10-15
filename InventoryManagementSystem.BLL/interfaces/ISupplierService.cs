using System.Threading.Tasks;
using InventoryManagementSystem.EntitiesLayer.Models;

namespace InventoryManagementSystem.BLL.interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<int> DeleteAsync(Supplier supplier);
        Task<int> AddAsync(Supplier supplier);
        Task<int> UpdateAsync(Supplier supplier);
        Task<Supplier> GetSupplierByIdAsync(int id);
    }
}
