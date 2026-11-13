using Bloomy.Api.DTOs;

namespace Bloomy.Api.Services
{
    public interface IUserService
    {
        // 1. التسجيل وتسجيل الدخول
        Task<(UserProfileDto? User, string? Error)> RegisterAsync(RegisterDto dto);
        Task<UserProfileDto?> LoginAsync(LoginDto dto);

        // 2. بيانات الملف الشخصي
        Task<UserProfileDto?> GetUserByIdAsync(int id);
        Task<UserProfileDto?> UpdateUserAsync(int id, UpdateUserDto dto);

        // 3. إدارة العناوين
        Task<List<AddressDto>?> GetUserAddressesAsync(int userId);
        Task<AddressDto?> AddAddressAsync(int userId, CreateUpdateAddressDto dto);
        Task<AddressDto?> UpdateAddressAsync(int userId, int addressId, CreateUpdateAddressDto dto);
        Task<bool> DeleteAddressAsync(int userId, int addressId);

        // 4. إدارة المفضلة
        Task<List<FavoriteDto>?> GetUserFavoritesAsync(int userId);
        Task<(FavoriteDto? Favorite, string? Error, bool NotFound)> AddFavoriteAsync(int userId, AddFavoriteDto dto);
        Task<bool> RemoveFavoriteAsync(int userId, int productId);
    }
}