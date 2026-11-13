using Microsoft.EntityFrameworkCore;
using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
using Bloomy.Api.Models;

namespace Bloomy.Api.Services
{
    public class UserService : IUserService
    {
        private readonly BloomyDbContext _context;

        public UserService(BloomyDbContext context)
        {
            _context = context;
        }

        // 1. إنشاء حساب جديد
        public async Task<(UserProfileDto? User, string? Error)> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == dto.Email.Trim().ToLower());

            if (emailExists)
            {
                return (null, "البريد الإلكتروني مسجل مسبقاً.");
            }

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim(),
                PasswordHash = dto.Password,
                Phone = dto.Phone?.Trim(),
                Role = "Customer",
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (MapToUserProfileDto(user), null);
        }

        // 2. تسجيل الدخول البسيط
        public async Task<UserProfileDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == dto.Email.Trim().ToLower() &&
                u.PasswordHash == dto.Password);

            if (user == null)
            {
                return null;
            }

            return MapToUserProfileDto(user);
        }

        // 3. عرض بيانات المستخدم
        public async Task<UserProfileDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToUserProfileDto(user);
        }

        // 4. تعديل بيانات المستخدم
        public async Task<UserProfileDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            user.FullName = dto.FullName.Trim();
            user.Phone = dto.Phone?.Trim();

            await _context.SaveChangesAsync();
            return MapToUserProfileDto(user);
        }

        // 5. عرض عناوين المستخدم
        public async Task<List<AddressDto>?> GetUserAddressesAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return null;
            }

            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .Select(a => new AddressDto
                {
                    AddressId = a.AddressId,
                    UserId = a.UserId,
                    Title = a.Title,
                    AddressLine = a.AddressLine
                })
                .ToListAsync();
        }

        // 6. إضافة عنوان جديد للمستخدم
        public async Task<AddressDto?> AddAddressAsync(int userId, CreateUpdateAddressDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return null;
            }

            var address = new Address
            {
                UserId = userId,
                Title = dto.Title.Trim(),
                AddressLine = dto.AddressLine.Trim()
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return new AddressDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                Title = address.Title,
                AddressLine = address.AddressLine
            };
        }

        // 7. تعديل عنوان موجود للمستخدم
        public async Task<AddressDto?> UpdateAddressAsync(int userId, int addressId, CreateUpdateAddressDto dto)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
            {
                return null;
            }

            address.Title = dto.Title.Trim();
            address.AddressLine = dto.AddressLine.Trim();

            await _context.SaveChangesAsync();

            return new AddressDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                Title = address.Title,
                AddressLine = address.AddressLine
            };
        }

        // 8. حذف عنوان
        public async Task<bool> DeleteAddressAsync(int userId, int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null)
            {
                return false;
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        // 9. عرض قائمة المفضلة للمستخدم
        public async Task<List<FavoriteDto>?> GetUserFavoritesAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return null;
            }

            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Join(_context.Products,
                    f => f.ProductId,
                    p => p.ProductId,
                    (f, p) => new FavoriteDto
                    {
                        UserId = f.UserId,
                        ProductId = f.ProductId,
                        ProductName = p.Name,
                        Price = p.Price,
                        MainImageUrl = p.MainImageUrl,
                        AddedAt = f.AddedAt
                    })
                .ToListAsync();
        }

        // 10. إضافة منتج إلى المفضلة
        public async Task<(FavoriteDto? Favorite, string? Error, bool NotFound)> AddFavoriteAsync(int userId, AddFavoriteDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return (null, "المستخدم غير موجود.", true);
            }

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                return (null, "المنتج غير موجود.", true);
            }

            var alreadyFavorited = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ProductId == dto.ProductId);

            if (alreadyFavorited)
            {
                return (null, "المنتج موجود بالفعل في قائمة المفضلة.", false);
            }

            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = dto.ProductId,
                AddedAt = DateTime.Now
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            var result = new FavoriteDto
            {
                UserId = favorite.UserId,
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                MainImageUrl = product.MainImageUrl,
                AddedAt = favorite.AddedAt
            };

            return (result, null, false);
        }

        // 11. حذف منتج من المفضلة
        public async Task<bool> RemoveFavoriteAsync(int userId, int productId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (favorite == null)
            {
                return false;
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        // دالة مساعدة لتحويل كيان User إلى UserProfileDto
        private static UserProfileDto MapToUserProfileDto(User user)
        {
            return new UserProfileDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}