using InventoryManagementSystem.BLL.Dto;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;
     

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task UpdateProfileAsync(string userId, EditProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Update name and phone number
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            // Handle Profile Picture upload
            if (dto.ProfilePicture != null)
            {
                try
                {
                    var profilePicPath = await UploadFile(dto.ProfilePicture, "images");
                    user.ProfilePictureUrl = profilePicPath;
                }
                catch (Exception ex)
                {
                    // Handle exception (log it or rethrow)
                    throw new Exception("Error uploading profile picture: " + ex.Message);
                }
            }

            // Handle Cover Photo upload
            if (dto.CoverPhoto != null)
            {
                try
                {
                    var coverPhotoPath = await UploadFile(dto.CoverPhoto, "images");
                    user.CoverPhotoUrl = coverPhotoPath;
                }
                catch (Exception ex)
                {
                    // Handle exception (log it or rethrow)
                    throw new Exception("Error uploading cover photo: " + ex.Message);
                }
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task<string> UploadFile(IFormFile file, string destinationFolder)
        {
            string uniqueFileName = string.Empty;

            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // استخدم CopyToAsync لرفع الملف بشكل غير متزامن
                }
            }

            return uniqueFileName;
        }


       
    }
}
