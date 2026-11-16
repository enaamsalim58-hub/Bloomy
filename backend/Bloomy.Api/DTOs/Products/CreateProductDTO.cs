using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs.Products
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0.01, 999999)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public string? MainImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}