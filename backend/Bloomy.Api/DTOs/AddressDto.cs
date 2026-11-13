namespace Bloomy.Api.DTOs
{
    public class AddressDto
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
    }
}