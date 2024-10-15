using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.EntitiesLayer.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PhotoName { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
