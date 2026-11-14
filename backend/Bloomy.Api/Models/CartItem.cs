using System.ComponentModel.DataAnnotations.Schema;
using Bloomy.Api.Models;

namespace Bloomy.Api.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? ProductSizeId { get; set; }
        public int? ProductColorId { get; set; }

        // ✅ التعديل: نخلي EF يتجاهل هذي الخاصية
        // لأن نوعها object وما ينفع تتخزن في قاعدة البيانات
        [NotMapped]
        public object? Product { get; set; }

        //إذا عندك خاصية User بنفس الشكل، ضيفي فوقها نفس السطر
        //[NotMapped]
        // public object? User { get; set; }
    }
}