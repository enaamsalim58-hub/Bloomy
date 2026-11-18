using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
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

        public async Task<List<CartItemResponseDTO>?> GetCartAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists) return null;

            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var productIds = cartItems.Select(c => c.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);

            var sizeIds = cartItems.Where(c => c.ProductSizeId.HasValue).Select(c => c.ProductSizeId!.Value).Distinct().ToList();
            var sizes = await _context.ProductSizes
                .Where(s => sizeIds.Contains(s.ProductSizeId))
                .ToDictionaryAsync(s => s.ProductSizeId);

            var colorIds = cartItems.Where(c => c.ProductColorId.HasValue).Select(c => c.ProductColorId!.Value).Distinct().ToList();
            var colors = await _context.ProductColors
                .Where(c => colorIds.Contains(c.ProductColorId))
                .ToDictionaryAsync(c => c.ProductColorId);

            var result = new List<CartItemResponseDTO>();

            foreach (var c in cartItems)
            {
                if (!products.TryGetValue(c.ProductId, out var p)) continue;

                decimal extraPrice = 0;
                string sizeName = "متوسط";
                if (c.ProductSizeId.HasValue && sizes.TryGetValue(c.ProductSizeId.Value, out var sz))
                {
                    sizeName = sz.Name;
                    extraPrice = sz.ExtraPrice;
                }

                string colorName = "افتراضي";
                if (c.ProductColorId.HasValue && colors.TryGetValue(c.ProductColorId.Value, out var col))
                {
                    colorName = col.ColorName;
                }

                var unitPrice = p.Price + extraPrice;

                result.Add(new CartItemResponseDTO
                {
                    CartItemId = c.CartItemId,
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    ProductPrice = unitPrice,
                    UnitPrice = unitPrice,
                    ProductImage = p.MainImageUrl,
                    ImageUrl = p.MainImageUrl,
                    Quantity = c.Quantity,
                    ProductSizeId = c.ProductSizeId,
                    SizeName = sizeName,
                    ProductColorId = c.ProductColorId,
                    ColorName = colorName,
                    Subtotal = unitPrice * c.Quantity
                });
            }

            return result;
        }

        public async Task<(bool Success, string? Error, bool NotFound, int CartItemId)> AddToCartAsync(int userId, AddToCartDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists) return (false, "المستخدم غير موجود.", true, 0);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == dto.ProductId);
            if (product == null) return (false, "المنتج غير موجود.", true, 0);
            if (!product.IsAvailable || product.StockQuantity <= 0)
                return (false, "المنتج غير متوفر حالياً.", false, 0);

            int? sizeId = dto.ProductSizeId;
            if (!sizeId.HasValue && !string.IsNullOrWhiteSpace(dto.SizeName))
            {
                var sizeEntity = await _context.ProductSizes
                    .FirstOrDefaultAsync(s => s.ProductId == dto.ProductId && s.Name == dto.SizeName.Trim());
                sizeId = sizeEntity?.ProductSizeId;
            }

            int? colorId = dto.ProductColorId;
            if (!colorId.HasValue && !string.IsNullOrWhiteSpace(dto.ColorName))
            {
                var colorEntity = await _context.ProductColors
                    .FirstOrDefaultAsync(c => c.ProductId == dto.ProductId && c.ColorName == dto.ColorName.Trim());
                colorId = colorEntity?.ProductColorId;
            }

            var item = await _context.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ProductId == dto.ProductId &&
                c.ProductSizeId == sizeId &&
                c.ProductColorId == colorId);

            int newQuantity = dto.Quantity + (item?.Quantity ?? 0);
            if (newQuantity > product.StockQuantity)
                return (false, "الكمية المطلوبة غير متوفرة في المخزون.", false, 0);

            if (item == null)
            {
                item = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    ProductSizeId = sizeId,
                    ProductColorId = colorId
                };
                _context.CartItems.Add(item);
            }
            else
            {
                item.Quantity = newQuantity;
            }

            await _context.SaveChangesAsync();
            return (true, null, false, item.CartItemId);
        }

        public async Task<(bool Success, string? Error, bool NotFound)> UpdateCartAsync(int userId, int cartItemId, UpdateCartDTO dto)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.CartItemId == cartItemId);
            if (item == null) return (false, "عنصر السلة غير موجود.", true);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
            if (product == null) return (false, "المنتج غير موجود.", true);
            if (dto.Quantity > product.StockQuantity)
                return (false, "الكمية المطلوبة غير متوفرة.", false);

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            return (true, null, false);
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.CartItemId == cartItemId);
            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}