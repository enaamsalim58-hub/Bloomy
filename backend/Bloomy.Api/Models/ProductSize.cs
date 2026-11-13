using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class ProductSize
    {
        public int ProductSizeId { get; set; }
        public int ProductId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }

        public Product Product { get; set; } = null!;
    }
}