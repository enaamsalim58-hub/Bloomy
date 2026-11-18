using Bloomy.Api.DTOs.Products;

namespace Bloomy.Api.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync(ProductFilterDTO? filter = null);
        Task<IEnumerable<ProductResponseDTO>> GetPagedAsync(int pageIndex, int pageSize);
        Task<ProductResponseDTO?> GetByIdAsync(int id);
        Task<ProductResponseDTO> CreateAsync(CreateProductDTO dto);
        Task<bool> UpdateAsync(int id, UpdateProductDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProductReviewDTO>?> GetReviewsAsync(int productId);
        Task<(ProductReviewDTO? Review, string? Error, bool NotFound)> AddReviewAsync(int productId, CreateReviewDTO dto);
    }
}