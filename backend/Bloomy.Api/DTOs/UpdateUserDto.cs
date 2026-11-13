using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }
    }
}