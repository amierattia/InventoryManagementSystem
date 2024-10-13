using InventoryManagementSystem.EntitiesLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.BLL.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?""':;{}|<>])(?=.*[a-zA-Z])(?=.*[0-9]).{6,}$", ErrorMessage = "Password must contain a special character, a letter, and a number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [StringLength(100)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public static class RegisterDtoExtensions
    {
        public static User ToUser(this RegisterDto dto)
        {
            return new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
        }
    }
}
