using Bloomy.Api.DTOs.Products;
using Bloomy.Api.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts([FromQuery] ProductFilterDTO? filter)
        {
            var products = await _service.GetAllAsync(filter);
            return Ok(products);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetPagedProducts(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var products = await _service.GetPagedAsync(pageIndex, pageSize);
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound(new { message = "«·„‰ Ã €Ì— „ÊÃÊœ." });
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> PostProduct(CreateProductDTO dto)
        {
            try
            {
                var product = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated) return NotFound(new { message = "«·„‰ Ã €Ì— „ÊÃÊœ." });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "«·„‰ Ã €Ì— „ÊÃÊœ." });
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> Search(
            [FromQuery] string? keyword,
            [FromQuery] string? term)
        {
            var searchText = !string.IsNullOrWhiteSpace(keyword) ? keyword : term;
            if (string.IsNullOrWhiteSpace(searchText))
                return BadRequest(new { message = "Ì—ÃÏ ≈œŒ«· ﬂ·„… «·»ÕÀ." });

            var filter = new ProductFilterDTO { SearchTerm = searchText };
            var products = await _service.GetAllAsync(filter);
            return Ok(products);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> Filter(
            [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? isAvailable)
        {
            var filter = new ProductFilterDTO
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                IsAvailable = isAvailable
            };
            var products = await _service.GetAllAsync(filter);
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}/reviews")]
        public async Task<IActionResult> GetProductReviews(int id)
        {
            var reviews = await _service.GetReviewsAsync(id);
            if (reviews == null) return NotFound(new { message = "«·„‰ Ã €Ì— „ÊÃÊœ." });
            return Ok(reviews);
        }

        [HttpPost("{id:int:min(1)}/reviews")]
        public async Task<IActionResult> AddProductReview(int id, [FromBody] CreateReviewDTO dto)
        {
            var (review, error, notFound) = await _service.AddReviewAsync(id, dto);
            if (notFound) return NotFound(new { message = error });
            if (error != null) return BadRequest(new { message = error });
            return StatusCode(201, review);
        }
    }
}