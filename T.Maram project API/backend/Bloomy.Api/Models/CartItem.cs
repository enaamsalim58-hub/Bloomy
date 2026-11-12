using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        public int? ProductSizeId { get; set; }
        public int? ProductColorId { get; set; }
    }
}