using Bloomy.Api.DTOs;

namespace Bloomy.Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemResponseDTO>?> GetCartAsync(int userId);
        Task<(bool Success, string? Error, bool NotFound, int CartItemId)> AddToCartAsync(int userId, AddToCartDto dto);
        Task<(bool Success, string? Error, bool NotFound)> UpdateCartAsync(int userId, int cartItemId, UpdateCartDTO dto);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
    }
}