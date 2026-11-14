using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
//using Bloomy.Api.DTOs.Cart;
using Bloomy.Api.Models;
using Bloomy.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Services
{
    public class CartService : ICartService
    {
        private readonly BloomyDbContext _context;

        public CartService(BloomyDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItemResponseDTO>> GetCartAsync(int userId)
        {
            return await _context.CartItems
                .Where(c => c.UserId == userId)
                .Join(
                    _context.Products,
                    c => c.ProductId,
                    p => p.ProductId,
                    (c, p) => new CartItemResponseDTO
                    {
                        CartItemId = c.CartItemId,
                        ProductId = p.ProductId,
                        ProductName = p.Name,
                        ProductPrice = p.Price,
                        ProductImage = p.MainImageUrl,
                        Quantity = c.Quantity,
                        ProductSizeId = c.ProductSizeId,
                        ProductColorId = c.ProductColorId,
                        Subtotal = p.Price * c.Quantity
                    })
                .ToListAsync();
        }

        public async Task<bool> AddToCartAsync(int userId, AddToCartDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);

            if (product == null || !product.IsAvailable)
                return false;

            var item = await _context.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ProductId == dto.ProductId &&
                c.ProductSizeId == dto.ProductSizeId &&
                c.ProductColorId == dto.ProductColorId);

            int quantity = dto.Quantity + (item?.Quantity ?? 0);

            if (quantity > product.StockQuantity)
                return false;

            if (item == null)
            {
                item = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    ProductSizeId = dto.ProductSizeId,
                    ProductColorId = dto.ProductColorId
                };

                _context.CartItems.Add(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCartAsync(
            int userId,
            int cartItemId,
            UpdateCartDTO dto)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.CartItemId == cartItemId);

            if (item == null)
                return false;

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

            if (product == null || dto.Quantity > product.StockQuantity)
                return false;

            item.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(
            int userId,
            int cartItemId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.CartItemId == cartItemId);

            if (item == null)
                return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}