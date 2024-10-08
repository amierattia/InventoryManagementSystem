using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace InventoryManagementSystem.BLL.Dto
{
    public class UpdateRolesDto
    {
        public List<IdentityRole> AvailableRoles { get; set; }  
        public string UserId { get; set; }
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }

   
    
}
