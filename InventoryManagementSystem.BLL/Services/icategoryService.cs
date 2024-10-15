using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.BLL.Services
{
    public interface ICategoryService
    {

         void Add(Category entity);

         void delete(Category entity);


         Category? GetById(int? id);

          Task<IEnumerable<Category>>GetAll();


         void Update(Category entity);

         int Save();
        
    }
}
