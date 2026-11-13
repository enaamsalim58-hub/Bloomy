namespace Bloomy.Api.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

// كلاس خاص بعملية الإضافة ليكون مرتباً ونظيفاً مثل المنتجات
namespace Bloomy.Api.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}