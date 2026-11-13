using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}