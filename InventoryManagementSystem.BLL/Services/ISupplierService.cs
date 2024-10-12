using InventoryManagementSystem.EntitiesLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
        int Delete(Supplier product);
        int Add(Supplier product);
        int Update(Supplier product);

        Supplier GetSupplierById(int id);
    }
}
