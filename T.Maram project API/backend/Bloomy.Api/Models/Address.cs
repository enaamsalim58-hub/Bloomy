using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Title { get; set; } = string.Empty; // „À·: «·„‰“·° «·⁄„·

        [Required, MaxLength(250)]
        public string AddressLine { get; set; } = string.Empty;
    }
}