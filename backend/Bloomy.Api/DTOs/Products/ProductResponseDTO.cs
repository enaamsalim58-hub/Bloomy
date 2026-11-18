namespace Bloomy.Api.DTOs.Products
{
    public class ProductResponseDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public string? MainImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public List<string> Images { get; set; } = new();
        public List<ProductSizeItemDTO> Sizes { get; set; } = new();
        public List<ProductColorItemDTO> Colors { get; set; } = new();
        public ProductOfferItemDTO? PriceOffer { get; set; }
        public List<ProductReviewDTO> Reviews { get; set; } = new();
    }

    public class ProductSizeItemDTO
    {
        public int ProductSizeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal ExtraPrice { get; set; }
    }

    public class ProductColorItemDTO
    {
        public int ProductColorId { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;
    }

    public class ProductOfferItemDTO
    {
        public int PriceOfferId { get; set; }
        public decimal NewPrice { get; set; }
        public string PromotionalText { get; set; } = string.Empty;
    }

    public class ProductReviewDTO
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class CreateReviewDTO
    {
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}