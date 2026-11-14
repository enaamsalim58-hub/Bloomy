
using System.ComponentModel.DataAnnotations;

namespace Bloomy.Api.DTOs
{
    public class UpdateCartDTO
    {
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
