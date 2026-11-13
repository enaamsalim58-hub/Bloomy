using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class PriceOffer
    {
        public int PriceOfferId { get; set; }
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
        [MaxLength(200)]
        public string PromotionalText { get; set; } = string.Empty;

        public Product Product { get; set; } = null!;
    }
}