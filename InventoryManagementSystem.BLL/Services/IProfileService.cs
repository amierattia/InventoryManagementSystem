using InventoryManagementSystem.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public interface IProfileService
    {
        Task UpdateProfileAsync(string userId, EditProfileDto dto);
    }
}
