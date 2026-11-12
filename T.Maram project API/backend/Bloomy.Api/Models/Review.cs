using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int ProductId { get; set; } // ÑÈØ ãÚ ãäÊÌ ÇáßÊÇáæÌ
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}