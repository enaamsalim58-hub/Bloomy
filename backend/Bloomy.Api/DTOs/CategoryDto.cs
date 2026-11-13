namespace Bloomy.Api.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // استبدلناها لتطابق جدول الأقسام في الخطة
    }
}

// كلاس خاص بعملية الإضافة والتعديل للأقسام
namespace Bloomy.Api.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // لتكون متناسقة مع إرسال رابط صورة القسم
    }
}