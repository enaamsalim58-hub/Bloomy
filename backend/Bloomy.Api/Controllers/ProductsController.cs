using Bloomy.Api.Data;
using Bloomy.Api.DTOs;
using Bloomy.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly BloomyDbContext _context;

        public ProductsController(BloomyDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.PriceOffer) // تضمين عرض السعر إن وجد
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    IsAvailable = p.IsAvailable,           // الحقل الجديد المطابق للخطة
                    MainImageUrl = p.MainImageUrl,         // التسمية الجديدة المطابقة للخطة
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty
                })
                .ToListAsync();

            return products;
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productDto = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages) // جلب صور المنتج المتعددة
                .Include(p => p.ProductSizes)  // جلب الأحجام
                .Include(p => p.ProductColors) // جلب الألوان
                .Include(p => p.PriceOffer)    // جلب عرض السعر
                .Include(p => p.Tags)          // جلب الوسوم
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    IsAvailable = p.IsAvailable,
                    MainImageUrl = p.MainImageUrl,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty
                    // ملاحظة: يمكنك إضافة قوائم الصور والأحجام هنا في الـ DTO الخاص بك إذا كنتِ تريدين إرجاعها في تفاصيل الشاشة الثانية
                })
                .FirstOrDefaultAsync();

            if (productDto == null)
            {
                return NotFound();
            }

            return productDto;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductCreateDto createDto)
        {
            // التحقق من وجود القسم مسبقاً في قاعدة البيانات
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == createDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("القسم المحدد غير موجود.");
            }

            // تحويل الـ DTO إلى كلاس المنتج الأساسي للحفظ (باستخدام الأسماء الجديدة الحقول)
            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                StockQuantity = createDto.StockQuantity,
                IsAvailable = createDto.IsAvailable,       // التحديث الجديد
                MainImageUrl = createDto.MainImageUrl,     // التحديث الجديد
                CategoryId = createDto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // إرجاع النتيجة بالصيغة المرتبة (ProductDto)
            var productDto = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductId == product.ProductId)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    IsAvailable = p.IsAvailable,
                    MainImageUrl = p.MainImageUrl,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, productDto);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductCreateDto updateDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // التحقق من أن القسم المحدث موجود فعلاً
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == updateDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("القسم المحدد غير موجود.");
            }

            // تحديث البيانات بالأسماء الجديدة
            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.Price = updateDto.Price;
            product.StockQuantity = updateDto.StockQuantity;
            product.IsAvailable = updateDto.IsAvailable;
            product.MainImageUrl = updateDto.MainImageUrl;
            product.CategoryId = updateDto.CategoryId;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}