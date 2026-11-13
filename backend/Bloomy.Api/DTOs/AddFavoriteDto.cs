using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs
{
    public class AddFavoriteDto
    {
        [Required(ErrorMessage = "رقم المنتج مطلوب")]
        public int ProductId { get; set; }
    }
}