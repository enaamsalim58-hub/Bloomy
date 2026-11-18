using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs
{
    public class CreateOrderDTO
    {
        [Required]
        [MaxLength(100)]
        public string ReceiverName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ReceiverPhone { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string DeliveryAddress { get; set; } = string.Empty;

        public decimal DeliveryFee { get; set; } = 20m;
        public decimal Discount { get; set; } = 0m;
    }
}