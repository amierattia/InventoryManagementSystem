using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.EntitiesLayer.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription{ get; set; }
        public decimal Price { get; set; }
        public int stockQuantity { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}
