using Microsoft.AspNetCore.Mvc;
using Bloomy.Api.DTOs;
using Bloomy.Api.Services;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // ==========================================================
        // 1. التسجيل وتسجيل الدخول البسيط
        // ==========================================================

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var (user, error) = await _userService.RegisterAsync(dto);

            if (error != null)
            {
                return BadRequest(new { message = error }); // 400
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user!.UserId }, user); // 201
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto);

            if (user == null)
            {
                return BadRequest(new { message = "البريد الإلكتروني أو كلمة المرور غير صحيحة." }); // 400
            }

            return Ok(user); // 200
        }

        // ==========================================================
        // 2. عرض وتعديل بيانات الملف الشخصي للمستخدم
        // ==========================================================

        // GET: api/users/{id}
        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." }); // 404
            }

            return Ok(user); // 200
        }

        // PUT: api/users/{id}
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto dto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, dto);

            if (updatedUser == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." }); // 404
            }

            return Ok(updatedUser); // 200
        }

        // ==========================================================
        // 3. إدارة عناوين التوصيل (Addresses)
        // ==========================================================

        // GET: api/users/{userId}/addresses
        [HttpGet("{userId:int:min(1)}/addresses")]
        public async Task<IActionResult> GetUserAddresses([FromRoute] int userId)
        {
            var addresses = await _userService.GetUserAddressesAsync(userId);

            if (addresses == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." }); // 404
            }

            return Ok(addresses); // 200
        }

        // POST: api/users/{userId}/addresses
        [HttpPost("{userId:int:min(1)}/addresses")]
        public async Task<IActionResult> AddAddress([FromRoute] int userId, [FromBody] CreateUpdateAddressDto dto)
        {
            var address = await _userService.AddAddressAsync(userId, dto);

            if (address == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." }); // 404
            }

            return StatusCode(201, address); // 201 Created
        }

        // PUT: api/users/{userId}/addresses/{addressId}
        [HttpPut("{userId:int:min(1)}/addresses/{addressId:int:min(1)}")]
        public async Task<IActionResult> UpdateAddress(
            [FromRoute] int userId,
            [FromRoute] int addressId,
            [FromBody] CreateUpdateAddressDto dto)
        {
            var updatedAddress = await _userService.UpdateAddressAsync(userId, addressId, dto);

            if (updatedAddress == null)
            {
                return NotFound(new { message = "المستخدم أو العنوان غير موجود." }); // 404
            }

            return Ok(updatedAddress); // 200
        }

        // DELETE: api/users/{userId}/addresses/{addressId}
        [HttpDelete("{userId:int:min(1)}/addresses/{addressId:int:min(1)}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int userId, [FromRoute] int addressId)
        {
            var deleted = await _userService.DeleteAddressAsync(userId, addressId);

            if (!deleted)
            {
                return NotFound(new { message = "المستخدم أو العنوان غير موجود." }); // 404
            }

            return Ok(new { message = "تم حذف العنوان بنجاح." }); // 200
        }

        // ==========================================================
        // 4. إدارة قائمة المفضلة (Favorites)
        // ==========================================================

        // GET: api/users/{userId}/favorites
        [HttpGet("{userId:int:min(1)}/favorites")]
        public async Task<IActionResult> GetUserFavorites([FromRoute] int userId)
        {
            var favorites = await _userService.GetUserFavoritesAsync(userId);

            if (favorites == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." }); // 404
            }

            return Ok(favorites); // 200
        }

        // POST: api/users/{userId}/favorites
        [HttpPost("{userId:int:min(1)}/favorites")]
        public async Task<IActionResult> AddFavorite([FromRoute] int userId, [FromBody] AddFavoriteDto dto)
        {
            var (favorite, error, notFound) = await _userService.AddFavoriteAsync(userId, dto);

            if (notFound)
            {
                return NotFound(new { message = error }); // 404
            }

            if (error != null)
            {
                return BadRequest(new { message = error }); // 400
            }

            return StatusCode(201, favorite); // 201 Created
        }

        // DELETE: api/users/{userId}/favorites/{productId}
        [HttpDelete("{userId:int:min(1)}/favorites/{productId:int:min(1)}")]
        public async Task<IActionResult> RemoveFavorite([FromRoute] int userId, [FromRoute] int productId)
        {
            var removed = await _userService.RemoveFavoriteAsync(userId, productId);

            if (!removed)
            {
                return NotFound(new { message = "المنتج غير موجود في قائمة المفضلة." }); // 404
            }

            return Ok(new { message = "تمت إزالة المنتج من المفضلة بنجاح." }); // 200
        }
    }
}