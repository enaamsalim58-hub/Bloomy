namespace Bloomy.Api.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }     // أضفناه ليتطابق مع جدول المنتجات
        public string? MainImageUrl { get; set; } // استبدلناها بـ MainImageUrl حسب جدول الخطة
        public int CategoryId { get; set; }
    }
}