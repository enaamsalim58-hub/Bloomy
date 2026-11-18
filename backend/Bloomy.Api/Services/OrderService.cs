using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
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

        public async Task<List<OrderResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return await MapOrdersWithProductsAsync(orders);
        }

        public async Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId, OrderStatus? status = null)
        {
            var query = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return await MapOrdersWithProductsAsync(orders);
        }

        public async Task<OrderResponseDTO?> GetOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            var mapped = await MapOrdersWithProductsAsync(new List<Order> { order });
            return mapped.FirstOrDefault();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDTO dto)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), dto.Status)) return false;

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null) return false;

            order.Status = dto.Status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(OrderResponseDTO? Order, string? Error, bool NotFound)> CreateOrderAsync(int userId, CreateOrderDTO dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists) return (null, "المستخدم غير موجود.", true);

            var cartItems = await _context.CartItems.Where(c => c.UserId == userId).ToListAsync();
            if (!cartItems.Any()) return (null, "السلة فارغة.", false);

            var productIds = cartItems.Select(c => c.ProductId).Distinct().ToList();
            var products = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

            var sizeIds = cartItems
                .Where(c => c.ProductSizeId.HasValue)
                .Select(c => c.ProductSizeId!.Value)
                .Distinct()
                .ToList();
            var sizesDict = await _context.ProductSizes
                .Where(s => sizeIds.Contains(s.ProductSizeId))
                .ToDictionaryAsync(s => s.ProductSizeId);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.New,
                ReceiverName = dto.ReceiverName,
                ReceiverPhone = dto.ReceiverPhone,
                DeliveryAddress = dto.DeliveryAddress,
                DeliveryFee = dto.DeliveryFee,
                Discount = dto.Discount,
                OrderItems = new List<OrderItem>()
            };

            decimal subtotal = 0;
            foreach (var cartItem in cartItems)
            {
                var product = products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                if (product == null) return (null, "أحد المنتجات في السلة غير موجود.", false);
                if (!product.IsAvailable || cartItem.Quantity > product.StockQuantity)
                    return (null, $"المنتج {product.Name} غير متوفر بالكمية المطلوبة.", false);

                decimal extraPrice = 0m;
                if (cartItem.ProductSizeId.HasValue && sizesDict.TryGetValue(cartItem.ProductSizeId.Value, out var size))
                {
                    extraPrice = size.ExtraPrice;
                }

                var unitPrice = product.Price + extraPrice;
                subtotal += unitPrice * cartItem.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = unitPrice
                });

                product.StockQuantity -= cartItem.Quantity;
                if (product.StockQuantity == 0) product.IsAvailable = false;
            }

            order.Subtotal = subtotal;
            order.Total = subtotal + order.DeliveryFee - order.Discount;

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            var created = await GetOrderAsync(order.OrderId);
            return (created, null, false);
        }

        private async Task<List<OrderResponseDTO>> MapOrdersWithProductsAsync(List<Order> orders)
        {
            var allProductIds = orders
                .SelectMany(o => o.OrderItems)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var productsDict = await _context.Products
                .Where(p => allProductIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);

            return orders.Select(o =>
            {
                var items = o.OrderItems.Select(i =>
                {
                    productsDict.TryGetValue(i.ProductId, out var prod);
                    return new OrderItemResponseDTO
                    {
                        OrderItemId = i.OrderItemId,
                        ProductId = i.ProductId,
                        ProductName = prod?.Name ?? "منتج",
                        ProductImage = prod?.MainImageUrl ?? "/Images/f.jpeg",
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.UnitPrice * i.Quantity
                    };
                }).ToList();

                return new OrderResponseDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    StatusArabic = GetArabicStatus(o.Status),
                    Subtotal = o.Subtotal,
                    DeliveryFee = o.DeliveryFee,
                    Discount = o.Discount,
                    Total = o.Total,
                    ReceiverName = o.ReceiverName,
                    ReceiverPhone = o.ReceiverPhone,
                    DeliveryAddress = o.DeliveryAddress,
                    ProductImage = items.FirstOrDefault()?.ProductImage ?? "/Images/f.jpeg",
                    Items = items
                };
            }).ToList();
        }

        private static string GetArabicStatus(OrderStatus status) => status switch
        {
            OrderStatus.New => "جديد",
            OrderStatus.Processing => "قيد التجهيز",
            OrderStatus.OutForDelivery => "قيد التوصيل",
            OrderStatus.Delivered => "تم التسليم",
            OrderStatus.Cancelled => "ملغي",
            _ => "جديد"
        };
    }
}