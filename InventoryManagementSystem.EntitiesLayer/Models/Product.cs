using System.Collections.Generic;

namespace InventoryManagementSystem.EntitiesLayer.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int stockQuantity { get; set; } 
        public int CategoryId { get; set; }
        public int? SupplierId { get; set; }

        // Navigation properties
        public Category? Category { get; set; } 
        public Supplier? Supplier { get; set; } 
        public ICollection<Stock>? Stocks { get; set; } 
    }
}
