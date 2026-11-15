using Bloomy.Api.Data;
using Bloomy.Api.DTOs.Categories;
using Bloomy.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly BloomyDbContext _context;

        public CategoryService(BloomyDbContext context)
        {
            _context = context;
        }

        // عرض جميع التصنيفات
        public async Task<IEnumerable<CategoryResponseDTO>> GetAllAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            return categories.Select(MapToResponse);
        }

        // عرض تصنيف حسب الرقم
        public async Task<CategoryResponseDTO?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            return category == null ? null : MapToResponse(category);
        }

        // إضافة تصنيف
        public async Task<CategoryResponseDTO> CreateAsync(
            CreateCategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ImageUrl = dto.ImageUrl
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return MapToResponse(category);
        }

        // تعديل تصنيف
        public async Task<bool> UpdateAsync(
            int id, UpdateCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            category.Name = dto.Name;
            category.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();

            return true;
        }

        // حذف تصنيف
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null || category.Products.Any())
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        // تحويل التصنيف إلى DTO للعرض
        private static CategoryResponseDTO MapToResponse(Category category)
        {
            return new CategoryResponseDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
                ProductsCount = category.Products?.Count ?? 0
            };
        }
    }
}