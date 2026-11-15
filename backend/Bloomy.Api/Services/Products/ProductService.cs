using Bloomy.Api.Data;
using Bloomy.Api.DTOs.Products;
using Bloomy.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly BloomyDbContext _context;

        public ProductService(BloomyDbContext context)
        {
            _context = context;
        }

        // عرض جميع المنتجات مع البحث والفلترة
        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync(
            ProductFilterDTO? filter = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    query = query.Where(p =>
                        p.Name.Contains(filter.SearchTerm) ||
                        (p.Description != null &&
                         p.Description.Contains(filter.SearchTerm)));
                }

                if (filter.CategoryId.HasValue)
                    query = query.Where(p =>
                        p.CategoryId == filter.CategoryId.Value);

                if (filter.MinPrice.HasValue)
                    query = query.Where(p =>
                        p.Price >= filter.MinPrice.Value);

                if (filter.MaxPrice.HasValue)
                    query = query.Where(p =>
                        p.Price <= filter.MaxPrice.Value);

                if (filter.IsAvailable.HasValue)
                    query = query.Where(p =>
                        p.StockQuantity > 0 == filter.IsAvailable.Value);

                query = filter.SortBy?.ToLower() switch
                {
                    "price" => filter.SortDescending
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),

                    "name" => filter.SortDescending
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name),

                    _ => query.OrderBy(p => p.ProductId)
                };
            }

            var products = await query.ToListAsync();

            return products.Select(MapToResponse);
        }

        // عرض منتج حسب الرقم
        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return product == null ? null : MapToResponse(product);
        }

        // إضافة منتج
        public async Task<ProductResponseDTO> CreateAsync(
            CreateProductDTO dto)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                throw new ArgumentException("التصنيف غير موجود.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                IsAvailable = dto.StockQuantity > 0,
                MainImageUrl = dto.MainImageUrl,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(product.ProductId))!;
        }

        // تعديل منتج
        public async Task<bool> UpdateAsync(
            int id, UpdateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                throw new ArgumentException("التصنيف غير موجود.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.IsAvailable = dto.StockQuantity > 0;
            product.MainImageUrl = dto.MainImageUrl;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            return true;
        }

        // حذف منتج
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        // تحويل المنتج إلى DTO للعرض
        private static ProductResponseDTO MapToResponse(Product product)
        {
            return new ProductResponseDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsAvailable = product.StockQuantity > 0,
                MainImageUrl = product.MainImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty
            };
        }
    }
}