using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
using Bloomy.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BloomyDbContext _context;

        public OrderController(BloomyDbContext context)
        {
            _context = context;
        }

        // POST: api/Order/2
        [HttpPost("{userId:int}")]
        public async Task<IActionResult> CreateOrder(
            int userId,
            [FromBody] CreateOrderDTO dto)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("المستخدم غير موجود.");

            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("السلة فارغة.");

            var productIds = cartItems
                .Select(c => c.ProductId)
                .Distinct()
                .ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            decimal subtotal = 0;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.New,
                ReceiverName = dto.ReceiverName,
                ReceiverPhone = dto.ReceiverPhone,
                DeliveryAddress = dto.DeliveryAddress,
                DeliveryFee = 0,
                Discount = 0
            };

            foreach (var cartItem in cartItems)
            {
                var product = products.FirstOrDefault(
                    p => p.ProductId == cartItem.ProductId);

                if (product == null)
                    return BadRequest("أحد المنتجات في السلة غير موجود.");

                if (!product.IsAvailable ||
                    cartItem.Quantity > product.StockQuantity)
                {
                    return BadRequest(
                        $"المنتج {product.Name} غير متوفر بالكمية المطلوبة.");
                }

                decimal itemSubtotal =
                    product.Price * cartItem.Quantity;

                subtotal += itemSubtotal;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                });

                // نقص المخزون
                product.StockQuantity -= cartItem.Quantity;
            }

            order.Subtotal = subtotal;
            order.Total = subtotal + order.DeliveryFee - order.Discount;

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "تم إنشاء الطلب بنجاح.",
                OrderId = order.OrderId,
                Total = order.Total
            });
        }

        // GET: api/Order/user/2
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // جلب أسماء المنتجات
            var allProductIds = orders
                .SelectMany(o => o.OrderItems)
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var productsDict = await _context.Products
                .Where(p => allProductIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId, p => p.Name);

            var result = orders.Select(o => new OrderResponseDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                Subtotal = o.Subtotal,
                DeliveryFee = o.DeliveryFee,
                Discount = o.Discount,
                Total = o.Total,
                ReceiverName = o.ReceiverName,
                ReceiverPhone = o.ReceiverPhone,
                DeliveryAddress = o.DeliveryAddress,

                Items = o.OrderItems.Select(i =>
                    new OrderItemResponseDTO
                    {
                        OrderItemId = i.OrderItemId,
                        ProductId = i.ProductId,
                        ProductName = productsDict.ContainsKey(i.ProductId)
                            ? productsDict[i.ProductId]
                            : "",
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.UnitPrice * i.Quantity
                    }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/Order/5
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound("الطلب غير موجود.");

            // جلب أسماء المنتجات
            var productIds = order.OrderItems
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var productsDict = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId, p => p.Name);

            var result = new OrderResponseDTO
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
                        ProductName = productsDict.ContainsKey(i.ProductId)
                            ? productsDict[i.ProductId]
                            : "",
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Subtotal = i.UnitPrice * i.Quantity
                    }).ToList()
            };

            return Ok(result);
        }

        // PUT: api/Order/5/status
        [HttpPut("{orderId:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
            int orderId,
            [FromBody] UpdateOrderStatusDTO dto)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), dto.Status))
                return BadRequest("حالة الطلب غير صحيحة.");

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound("الطلب غير موجود.");

            order.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok("تم تحديث حالة الطلب بنجاح.");
        }
    }
}