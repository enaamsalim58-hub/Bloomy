namespace Bloomy.Api.Models
{
    public class Favorite
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ProductId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}