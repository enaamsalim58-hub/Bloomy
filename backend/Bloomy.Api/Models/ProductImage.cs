using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public Product Product { get; set; } = null!;
    }
}