using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs.Categories
{
    public class UpdateCategoryDTO
    {
        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}