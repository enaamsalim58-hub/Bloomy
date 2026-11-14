
using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs
{
    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        public int? ProductSizeId { get; set; }

        public int? ProductColorId { get; set; }
    }
}
