using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.BLL.Services;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.interfaces
{
    public interface IProfileService
    {
        Task<UpdateProfileResult> UpdateProfileAsync(string userId, EditProfileDto dto);
    }
}
