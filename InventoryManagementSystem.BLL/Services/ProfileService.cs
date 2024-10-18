using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using InventoryManagementSystem.BLL.interfaces;

namespace InventoryManagementSystem.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UpdateProfileResult> UpdateProfileAsync(string userId, EditProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return UpdateProfileResult.UserNotFound;
            }
            string mail = dto.Email;
            if (mail != null)
            {
                if (await _userManager.FindByEmailAsync(dto.Email) != null)
                {
                    return UpdateProfileResult.EmailExists;
                }
            }

            // Update user properties
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;

            // Handle file uploads
            user.ProfilePictureUrl = await HandleFileUpload(dto.ProfilePicture, user.ProfilePictureUrl);
            user.CoverPhotoUrl = await HandleFileUpload(dto.CoverPhoto, user.CoverPhotoUrl);

            await _userManager.UpdateAsync(user);
            return UpdateProfileResult.Success;
        }

        private async Task<string> HandleFileUpload(IFormFile file, string existingUrl)
        {
            if (file == null || file.Length == 0)
            {
                return existingUrl; // Return existing URL if no new file is uploaded
            }

            try
            {
                var filePath = await UploadFile(file, "images");
                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file: " + ex.Message);
            }
        }

        public async Task<string> UploadFile(IFormFile file, string destinationFolder)
        {
            string uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
            string uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
    }

    public enum UpdateProfileResult
    {
        Success,
        UserNotFound,
        EmailExists
    }
}
