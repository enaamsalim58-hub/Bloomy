using Bloomy.Api.DTOs.Categories;

namespace Bloomy.Api.Services.Products
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllAsync();

        Task<CategoryResponseDTO?> GetByIdAsync(int id);

        Task<CategoryResponseDTO> CreateAsync(CreateCategoryDTO dto);

        Task<bool> UpdateAsync(int id, UpdateCategoryDTO dto);

        Task<bool> DeleteAsync(int id);
    }
}