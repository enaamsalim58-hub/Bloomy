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

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts(
            [FromQuery] ProductFilterDTO? filter)
        {
            var products = await _service.GetAllAsync(filter);
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> PostProduct(
            CreateProductDTO dto)
        {
            try
            {
                var product = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetProduct),
                    new { id = product.ProductId },
                    product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(
            int id, UpdateProductDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // GET: api/Products/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> Search(
            [FromQuery] string term)
        {
            var filter = new ProductFilterDTO
            {
                SearchTerm = term
            };

            var products = await _service.GetAllAsync(filter);
            return Ok(products);
        }

        // GET: api/Products/filter
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
    }
}