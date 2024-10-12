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
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationContext _context;
        public SupplierService(ApplicationContext context)
        {

            _context = context;

        }
        public IEnumerable<Supplier> GetAll()
        {
            return _context.Suppliers.ToList();
        }
        public int Add(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            return _context.SaveChanges();
        }

        public int Delete(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            return _context.SaveChanges();
        }

        public int Update(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            return _context.SaveChanges();
        }



        Supplier ISupplierService.GetSupplierById(int id)
        {
            return _context.Find<Supplier>(id);
        }
    }
}
