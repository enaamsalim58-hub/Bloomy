namespace Bloomy.Api.DTOs.Categories
{
    public class CategoryResponseDTO
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public int ProductsCount { get; set; }
    }
}