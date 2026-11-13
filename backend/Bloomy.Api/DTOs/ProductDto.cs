namespace Bloomy.Api.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }        // أضفناه ليعرض حالة توفر المنتج
        public string? MainImageUrl { get; set; }    // استبدلناها بـ MainImageUrl لتطابق جدول الخطة
        public string CategoryName { get; set; } = string.Empty; // لعرض اسم القسم مباشرة
    }
}