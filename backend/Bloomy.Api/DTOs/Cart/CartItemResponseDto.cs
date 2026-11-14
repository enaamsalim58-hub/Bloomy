
namespace Bloomy.Api.DTOs
{
    public class CartItemResponseDTO
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; }

        public string? ProductImage { get; set; }

        public int Quantity { get; set; }

        public int? ProductSizeId { get; set; }

        public int? ProductColorId { get; set; }

        public decimal Subtotal { get; set; }
    }
}
