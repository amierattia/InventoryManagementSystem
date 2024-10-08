using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.EntitiesLayer.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
