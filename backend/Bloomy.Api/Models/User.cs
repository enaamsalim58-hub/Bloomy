using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty; // ›—Ìœ

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; } = "Customer"; // Admin √Ê Customer

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ⁄·«ﬁ«  «·„” Œœ„ „⁄ «·Ãœ«Ê· «·√Œ—Ï
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}