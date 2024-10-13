using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.EntitiesLayer.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
      

    }
}
