namespace Bloomy.Api.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }        // حقل أُضيف لتطابق جدول الخطة
        public string? MainImageUrl { get; set; }    // استبدلناها بـ MainImageUrl حسب جدول الخطة

        // العلاقة مع القسم (Category)
        public Category? Category { get; set; }

        // قوائم العلاقات مع جداول الكتالوج (الخاصة بكِ)
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public PriceOffer? PriceOffer { get; set; } // العلاقة 1:1
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        // قوائم الجداول المشتركة مع الطالبة رقم 5 (يجب وضعها كفارغة لكي يبنى المشروع بدون أخطاء حتى تملؤها هي)
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}