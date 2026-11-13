using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class ProductColor
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        [Required, MaxLength(50)]
        public string ColorName { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string HexCode { get; set; } = string.Empty;

        public Product Product { get; set; } = null!;
    }
}