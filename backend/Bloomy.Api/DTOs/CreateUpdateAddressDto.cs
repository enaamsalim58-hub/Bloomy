using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs
{
    public class CreateUpdateAddressDto
    {
        [Required(ErrorMessage = "عنوان المكان مطلوب مثل: المنزل أو العمل")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "تفاصيل العنوان مطلوبة")]
        [MaxLength(250)]
        public string AddressLine { get; set; } = string.Empty;
    }
}