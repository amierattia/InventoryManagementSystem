using InventoryManagementSystem.EntitiesLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();

        Product GetProductById(int id);
        int Delete(Product product);
        int Add(Product product);
        int Update(Product product);

    }
}
