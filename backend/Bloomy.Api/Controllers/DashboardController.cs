using Bloomy.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Controllers
{
    [Route("api/dashboard")]
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
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var availableProducts = await _context.Products.Where(p => p.IsAvailable && p.StockQuantity > 0).CountAsync();

            // حساب إجمالي المبيعات وعدد الطلبات والعملاء (إذا كانت جداول الطلبات والمستخدمين متوفرة في السياق)
            var totalOrders = _context.Model.FindEntityType(typeof(Models.Order)) != null ? await _context.Set<Models.Order>().CountAsync() : 0;
            var totalUsers = _context.Model.FindEntityType(typeof(Models.User)) != null ? await _context.Set<Models.User>().CountAsync() : 0;
            var totalSales = _context.Model.FindEntityType(typeof(Models.Order)) != null ? await _context.Set<Models.Order>().SumAsync(o => (decimal?)o.Total) ?? 0 : 0;

            return Ok(new
            {
                totalProducts,
                totalCategories,
                availableProducts,
                totalOrders,
                totalUsers,
                totalSales
            });
        }

        // GET: /api/dashboard/sales-monthly
        [HttpGet("sales-monthly")]
        public async Task<IActionResult> GetMonthlySales()
        {
            // التحقق من وجود جدول الطلبات لتجنب الأخطاء إذا لم تقم الطالبة 5 بدمجه بعد
            var orderEntity = _context.Model.FindEntityType(typeof(Models.Order));
            if (orderEntity == null)
            {
                return Ok(new List<object>()); // إرجاع قائمة فارغة مؤقتاً
            }

            var monthlySales = await _context.Set<Models.Order>()
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSales = g.Sum(o => o.Total),
                    OrderCount = g.Count()
                })
                .OrderBy(s => s.Year).ThenBy(s => s.Month)
                .ToListAsync();

            return Ok(monthlySales);
        }

        // GET: /api/dashboard/orders-by-status
        [HttpGet("orders-by-status")]
        public async Task<IActionResult> GetOrdersByStatus()
        {
            var orderEntity = _context.Model.FindEntityType(typeof(Models.Order));
            if (orderEntity == null)
            {
                return Ok(new List<object>());
            }

            var ordersByStatus = await _context.Set<Models.Order>()
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(ordersByStatus);
        }
    }
}