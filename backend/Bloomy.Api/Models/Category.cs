namespace Bloomy.Api.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // العلاقة: القسم الواحد يحتوي على عدة منتجات
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}