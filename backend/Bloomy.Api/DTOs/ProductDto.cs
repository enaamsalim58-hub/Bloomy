namespace Bloomy.Api.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty; // لعرض اسم القسم مباشرة دون الحاجة لعرض الـ ID المعقد
    }
}