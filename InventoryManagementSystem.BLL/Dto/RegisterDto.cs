using InventoryManagementSystem.EntitiesLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.BLL.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "يجب أن تكون كلمة المرور 6 أحرف على الأقل")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?""':;{}|<>])(?=.*[a-zA-Z])(?=.*[0-9]).{6,}$", ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف خاص، وحرف أبجدي، ورقم.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [StringLength(100)]
        [Compare("Password", ErrorMessage = "كلمات المرور لا تتطابق")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public static class RegisterDtoExtensions
    {
        public static User ToUser(this RegisterDto dto)
        {
            return new User
            {
                Name = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) // تأكد من تشفير كلمة المرور
            };
        }
    }
}
