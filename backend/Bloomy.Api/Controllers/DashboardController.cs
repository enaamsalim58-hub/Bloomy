using Bloomy.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly BloomyDbContext _context;

        public DashboardController(BloomyDbContext context)
        {
            _context = context;
        }

        // GET: /api/dashboard/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            // حساب الإحصائيات الخاصة بالكتالوج المتاحة لديكِ حالياً
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var availableProducts = await _context.Products.Where(p => p.StockQuantity > 0).CountAsync();

            return Ok(new
            {
                totalProducts,
                totalCategories,
                availableProducts
            });
        }
    }
}