using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
//using Bloomy.Api.DTOs.Orders;
using Bloomy.Api.Models;
using Bloomy.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly BloomyDbContext _context;

        public OrderService(BloomyDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(MapOrder).ToList();
        }

        public async Task<OrderResponseDTO?> GetOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            return order == null ? null : MapOrder(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(
            int orderId,
            UpdateOrderStatusDTO dto)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), dto.Status))
                return false;

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return false;

            order.Status = dto.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        private static OrderResponseDTO MapOrder(Order order)
        {
            return new OrderResponseDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Subtotal = order.Subtotal,
                DeliveryFee = order.DeliveryFee,
                Discount = order.Discount,
                Total = order.Total,
                ReceiverName = order.ReceiverName,
                ReceiverPhone = order.ReceiverPhone,
                DeliveryAddress = order.DeliveryAddress,

                Items = order.OrderItems.Select(i =>
                    new OrderItemResponseDTO
                    {
                        OrderItemId = i.OrderItemId,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.UnitPrice * i.Quantity
                    }).ToList()
            };
        }

        public async Task<OrderResponseDTO?> CreateOrderAsync(int userId, CreateOrderDTO dto)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId).ToListAsync();
            if (!cartItems.Any()) return null;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.New,
                ReceiverName = dto.ReceiverName,
                ReceiverPhone = dto.ReceiverPhone,
                DeliveryAddress = dto.DeliveryAddress,
                DeliveryFee = 0,
                Discount = 0,
                OrderItems = new List<OrderItem>()
            };

            decimal subtotal = 0;
            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);
                if (product == null || cartItem.Quantity > product.StockQuantity)
                    return null;

                subtotal += product.Price * cartItem.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                });

                product.StockQuantity -= cartItem.Quantity;
            }

            order.Subtotal = subtotal;
            order.Total = subtotal + order.DeliveryFee - order.Discount;

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return MapOrder(order);
        }
    }
}