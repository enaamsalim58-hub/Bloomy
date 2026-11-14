
//using Bloomy.Api.DTOs.Orders;
using Bloomy.Api.Models;

namespace Bloomy.Api.DTOs
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }

        public string ReceiverName { get; set; } = string.Empty;

        public string ReceiverPhone { get; set; } = string.Empty;

        public string DeliveryAddress { get; set; } = string.Empty;

        public List<OrderItemResponseDTO> Items { get; set; } = new();
    }
}
