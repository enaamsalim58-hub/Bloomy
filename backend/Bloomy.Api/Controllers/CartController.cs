using Bloomy.Api.DTOs;
using Bloomy.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId:int:min(1)}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var items = await _cartService.GetCartAsync(userId);
            if (items == null) return NotFound(new { message = "المستخدم غير موجود." });
            return Ok(new { UserId = userId, Items = items, Total = items.Sum(i => i.Subtotal) });
        }

        [HttpPost("{userId:int:min(1)}")]
        public async Task<IActionResult> AddToCart(int userId, [FromBody] AddToCartDto dto)
        {
            var (success, error, notFound, cartItemId) = await _cartService.AddToCartAsync(userId, dto);
            if (notFound) return NotFound(new { message = error });
            if (!success) return BadRequest(new { message = error });
            return Ok(new { Message = "تمت إضافة المنتج إلى السلة بنجاح.", CartItemId = cartItemId });
        }

        [HttpPut("{userId:int:min(1)}/{cartItemId:int:min(1)}")]
        public async Task<IActionResult> UpdateCart(int userId, int cartItemId, [FromBody] UpdateCartDTO dto)
        {
            var (success, error, notFound) = await _cartService.UpdateCartAsync(userId, cartItemId, dto);
            if (notFound) return NotFound(new { message = error });
            if (!success) return BadRequest(new { message = error });
            return Ok(new { message = "تم تحديث الكمية بنجاح." });
        }

        [HttpDelete("{userId:int:min(1)}/{cartItemId:int:min(1)}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int cartItemId)
        {
            var removed = await _cartService.RemoveFromCartAsync(userId, cartItemId);
            if (!removed) return NotFound(new { message = "عنصر السلة غير موجود." });
            return Ok(new { message = "تم حذف المنتج من السلة." });
        }
    }
}