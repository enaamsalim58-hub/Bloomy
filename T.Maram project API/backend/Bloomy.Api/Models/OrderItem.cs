using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; } // ”⁄— «·„‰ Ã Êﬁ  «·ÿ·»
    }
}