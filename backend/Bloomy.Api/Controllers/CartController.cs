using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
using Bloomy.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly BloomyDbContext _context;

        public CartController(BloomyDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart/2
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("المستخدم غير موجود.");

            var items = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Join(
                    _context.Products,
                    cart => cart.ProductId,
                    product => product.ProductId,
                    (cart, product) => new CartItemResponseDTO
                    {
                        CartItemId = cart.CartItemId,
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        ProductImage = product.MainImageUrl,
                        Quantity = cart.Quantity,
                        ProductSizeId = cart.ProductSizeId,
                        ProductColorId = cart.ProductColorId,
                        Subtotal = product.Price * cart.Quantity
                    })
                .ToListAsync();

            return Ok(new
            {
                UserId = userId,
                Items = items,
                Total = items.Sum(i => i.Subtotal)
            });
        }

        // POST: api/Cart/2
        [HttpPost("{userId:int}")]
        public async Task<IActionResult> AddToCart(
            int userId,
            [FromBody] AddToCartDto dto)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("المستخدم غير موجود.");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);

            if (product == null)
                return NotFound("المنتج غير موجود.");

            if (!product.IsAvailable)
                return BadRequest("المنتج غير متوفر حاليًا.");

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ProductId == dto.ProductId &&
                c.ProductSizeId == dto.ProductSizeId &&
                c.ProductColorId == dto.ProductColorId);

            int newQuantity = dto.Quantity +
                (cartItem?.Quantity ?? 0);

            if (newQuantity > product.StockQuantity)
                return BadRequest("الكمية المطلوبة غير متوفرة في المخزون.");

            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    ProductSizeId = dto.ProductSizeId,
                    ProductColorId = dto.ProductColorId
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "تمت إضافة المنتج إلى السلة بنجاح.",
                CartItemId = cartItem.CartItemId
            });
        }

        // PUT: api/Cart/2/5
        [HttpPut("{userId:int}/{cartItemId:int}")]
        public async Task<IActionResult> UpdateCart(
            int userId,
            int cartItemId,
            [FromBody] UpdateCartDTO dto)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.CartItemId == cartItemId &&
                    c.UserId == userId);

            if (cartItem == null)
                return NotFound("عنصر السلة غير موجود.");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);

            if (product == null)
                return NotFound("المنتج غير موجود.");

            if (dto.Quantity > product.StockQuantity)
                return BadRequest("الكمية المطلوبة غير متوفرة.");

            cartItem.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();

            return Ok("تم تحديث الكمية بنجاح.");
        }

        // DELETE: api/Cart/2/5
        [HttpDelete("{userId:int}/{cartItemId:int}")]
        public async Task<IActionResult> RemoveFromCart(
            int userId,
            int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.CartItemId == cartItemId &&
                    c.UserId == userId);

            if (cartItem == null)
                return NotFound("عنصر السلة غير موجود.");

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok("تم حذف المنتج من السلة.");
        }
    }
}