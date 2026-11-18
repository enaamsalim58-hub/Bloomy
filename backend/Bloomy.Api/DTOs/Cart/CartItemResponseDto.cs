namespace Bloomy.Api.DTOs
{
    public class CartItemResponseDTO
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductImage { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int? ProductSizeId { get; set; }
        public string SizeName { get; set; } = "متوسط";
        public int? ProductColorId { get; set; }
        public string ColorName { get; set; } = "افتراضي";
        public decimal Subtotal { get; set; }
    }
}