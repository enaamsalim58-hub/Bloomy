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

        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync(ProductFilterDTO? filter = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.PriceOffer)
                .Include(p => p.Reviews)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    query = query.Where(p => p.Name.Contains(filter.SearchTerm) ||
                        (p.Description != null && p.Description.Contains(filter.SearchTerm)));
                }
                if (filter.CategoryId.HasValue)
                    query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
                if (filter.MinPrice.HasValue)
                    query = query.Where(p => p.Price >= filter.MinPrice.Value);
                if (filter.MaxPrice.HasValue)
                    query = query.Where(p => p.Price <= filter.MaxPrice.Value);
                if (filter.IsAvailable.HasValue)
                    query = query.Where(p => (p.StockQuantity > 0) == filter.IsAvailable.Value);

                query = filter.SortBy?.ToLower() switch
                {
                    "price" => filter.SortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "name" => filter.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    _ => query.OrderBy(p => p.ProductId)
                };
            }

            var products = await query.ToListAsync();
            return products.Select(MapToResponse);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetPagedAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.PriceOffer)
                .Include(p => p.Reviews)
                .OrderBy(p => p.ProductId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return products.Select(MapToResponse);
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductColors)
                .Include(p => p.PriceOffer)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return product == null ? null : MapToResponse(product);
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
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

        public async Task<bool> UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
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

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductReviewDTO>?> GetReviewsAsync(int productId)
        {
            var exists = await _context.Products.AnyAsync(p => p.ProductId == productId);
            if (!exists) return null;

            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewId)
                .Select(r => new ProductReviewDTO
                {
                    ReviewId = r.ReviewId,
                    ProductId = r.ProductId,
                    UserId = r.UserId,
                    UserName = r.User != null ? r.User.FullName : "عميل Bloomy",
                    Rating = r.Rating,
                    Comment = r.Comment
                })
                .ToListAsync();
        }

        public async Task<(ProductReviewDTO? Review, string? Error, bool NotFound)> AddReviewAsync(int productId, CreateReviewDTO dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                return (null, "التقييم يجب أن يكون بين 1 و 5.", false);

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == productId);
            if (!productExists)
                return (null, "المنتج غير موجود.", true);

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return (null, "المستخدم غير موجود.", true);

            var review = new Review
            {
                ProductId = productId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment?.Trim()
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var result = new ProductReviewDTO
            {
                ReviewId = review.ReviewId,
                ProductId = review.ProductId,
                UserId = review.UserId,
                UserName = user.FullName,
                Rating = review.Rating,
                Comment = review.Comment
            };

            return (result, null, false);
        }

        private static ProductResponseDTO MapToResponse(Product product)
        {
            var gallery = new List<string>();
            if (!string.IsNullOrWhiteSpace(product.MainImageUrl))
                gallery.Add(product.MainImageUrl);

            if (product.ProductImages != null)
            {
                foreach (var img in product.ProductImages)
                {
                    if (!string.IsNullOrWhiteSpace(img.ImageUrl) && !gallery.Contains(img.ImageUrl))
                        gallery.Add(img.ImageUrl);
                }
            }

            var reviews = product.Reviews?.Select(r => new ProductReviewDTO
            {
                ReviewId = r.ReviewId,
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User?.FullName ?? "عميل Bloomy",
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList() ?? new List<ProductReviewDTO>();

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
                CategoryName = product.Category?.Name ?? string.Empty,
                AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 5.0,
                ReviewsCount = reviews.Count,
                Images = gallery,
                Sizes = product.ProductSizes?.Select(s => new ProductSizeItemDTO
                {
                    ProductSizeId = s.ProductSizeId,
                    Name = s.Name,
                    ExtraPrice = s.ExtraPrice
                }).ToList() ?? new(),
                Colors = product.ProductColors?.Select(c => new ProductColorItemDTO
                {
                    ProductColorId = c.ProductColorId,
                    ColorName = c.ColorName,
                    HexCode = c.HexCode
                }).ToList() ?? new(),
                PriceOffer = product.PriceOffer == null ? null : new ProductOfferItemDTO
                {
                    PriceOfferId = product.PriceOffer.PriceOfferId,
                    NewPrice = product.PriceOffer.NewPrice,
                    PromotionalText = product.PriceOffer.PromotionalText
                },
                Reviews = reviews
            };
        }
    }
}