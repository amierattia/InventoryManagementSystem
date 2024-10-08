using Microsoft.AspNetCore.Identity;


namespace InventoryManagementSystem.EntitiesLayer.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
