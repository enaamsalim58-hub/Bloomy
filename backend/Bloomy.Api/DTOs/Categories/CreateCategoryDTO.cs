using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
    }
}