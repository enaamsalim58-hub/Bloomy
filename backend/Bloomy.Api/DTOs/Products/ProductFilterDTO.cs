namespace Bloomy.Api.DTOs.Products
{
    public class ProductFilterDTO
    {
        public string? SearchTerm { get; set; }

        public int? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public bool? IsAvailable { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; }
    }
}