using Bloomy.Api.DTOs;
//using Bloomy.Api.DTOs.Cart;

namespace Bloomy.Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemResponseDTO>> GetCartAsync(int userId);

        Task<bool> AddToCartAsync(int userId, AddToCartDto dto);

        Task<bool> UpdateCartAsync(
            int userId,
            int cartItemId,
            UpdateCartDTO dto);

        Task<bool> RemoveFromCartAsync(
            int userId,
            int cartItemId);
    }
}