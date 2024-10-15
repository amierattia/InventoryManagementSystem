using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementSystem.BLL.Dto;
using Microsoft.AspNetCore.Http;

namespace InventoryManagementSystem.BLL.Dto
{
    public class EditProfileDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public IFormFile CoverPhoto { get; set; }
    }

}
