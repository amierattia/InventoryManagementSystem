<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Identity;


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
=======
﻿using Microsoft.AspNetCore.Identity;


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
>>>>>>> 5e9904703590e84da3312971fd01f46e05187cb4
