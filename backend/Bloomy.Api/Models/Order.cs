using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; } = OrderStatus.New;

        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string ReceiverPhone { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string DeliveryAddress { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}