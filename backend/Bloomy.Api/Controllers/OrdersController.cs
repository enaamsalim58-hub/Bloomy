using Bloomy.Api.DTOs;
using Bloomy.Api.Models;
using Bloomy.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/Order")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost("{userId:int:min(1)}")]
        public async Task<IActionResult> CreateOrder(int userId, [FromBody] CreateOrderDTO dto)
        {
            var (order, error, notFound) = await _orderService.CreateOrderAsync(userId, dto);
            if (notFound) return NotFound(new { message = error });
            if (error != null) return BadRequest(new { message = error });
            return StatusCode(201, new
            {
                Message = "تم إنشاء الطلب بنجاح.",
                OrderId = order!.OrderId,
                Total = order.Total,
                Order = order
            });
        }

        [HttpGet("user/{userId:int:min(1)}")]
        public async Task<IActionResult> GetUserOrders(int userId, [FromQuery] OrderStatus? status)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId, status);
            return Ok(orders);
        }

        [HttpGet("{orderId:int:min(1)}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);
            if (order == null) return NotFound(new { message = "الطلب غير موجود." });
            return Ok(order);
        }

        [HttpPut("{orderId:int:min(1)}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO dto)
        {
            var updated = await _orderService.UpdateOrderStatusAsync(orderId, dto);
            if (!updated) return BadRequest(new { message = "الطلب غير موجود أو حالة الطلب غير صحيحة." });
            return Ok(new { message = "تم تحديث حالة الطلب بنجاح." });
        }
    }
}