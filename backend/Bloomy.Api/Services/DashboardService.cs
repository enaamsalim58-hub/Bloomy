using Bloomy.Api.Data;
using Bloomy.Api.DTOs.Dashboard;
using Bloomy.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly BloomyDbContext _context;

        public DashboardService(BloomyDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var availableProducts = await _context.Products.CountAsync(p => p.IsAvailable && p.StockQuantity > 0);
            var totalOrders = await _context.Orders.CountAsync();
            var totalCustomers = await _context.Users.CountAsync(u => u.Role == "Customer" || u.Role == "customer");
            var totalUsers = await _context.Users.CountAsync();

            var totalSales = await _context.Orders
                .Where(o => o.Status != OrderStatus.Cancelled)
                .SumAsync(o => (decimal?)o.Total) ?? 0m;

            return new DashboardStatsDto
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                TotalUsers = totalUsers,
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                AvailableProducts = availableProducts
            };
        }

        public async Task<List<OrderStatusSummaryDto>> GetOrdersByStatusAsync()
        {
            var counts = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);

            return new List<OrderStatusSummaryDto>
            {
                new() { Status = "OutForDelivery", Label = "قيد التوصيل", Count = counts.GetValueOrDefault(OrderStatus.OutForDelivery, 0), Color = "#f59e0b" },
                new() { Status = "Delivered", Label = "تم التسليم", Count = counts.GetValueOrDefault(OrderStatus.Delivered, 0), Color = "#10b981" },
                new() { Status = "Processing", Label = "قيد التجهيز", Count = counts.GetValueOrDefault(OrderStatus.Processing, 0), Color = "#994b94" },
                new() { Status = "New", Label = "جديد", Count = counts.GetValueOrDefault(OrderStatus.New, 0), Color = "#3b82f6" },
                new() { Status = "Cancelled", Label = "ملغي", Count = counts.GetValueOrDefault(OrderStatus.Cancelled, 0), Color = "#ef4444" }
            };
        }

        public async Task<List<MonthlySalesDto>> GetMonthlySalesAsync()
        {
            string[] arabicMonths = { "", "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };

            var raw = await _context.Orders
                .Where(o => o.Status != OrderStatus.Cancelled)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new { Month = g.Key, Amount = g.Sum(x => x.Total) })
                .OrderBy(x => x.Month)
                .ToListAsync();

            if (!raw.Any())
            {
                return new List<MonthlySalesDto>
                {
                    new() { MonthNumber = DateTime.Now.Month, Month = arabicMonths[DateTime.Now.Month], Amount = 0, Percent = 0 }
                };
            }

            var maxAmount = raw.Max(x => x.Amount);

            return raw.Select(x => new MonthlySalesDto
            {
                MonthNumber = x.Month,
                Month = arabicMonths[x.Month],
                Amount = x.Amount,
                Percent = maxAmount > 0 ? (int)Math.Round((x.Amount / maxAmount) * 100) : 0
            }).ToList();
        }

        public async Task<List<ManagerCustomerDto>> GetCustomersAsync()
        {
            var customers = await _context.Users
                .Where(u => u.Role == "Customer" || u.Role == "customer")
                .ToListAsync();

            var customerIds = customers.Select(c => c.UserId).ToList();

            var orderStats = await _context.Orders
                .Where(o => customerIds.Contains(o.UserId))
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalOrders = g.Count(),
                    TotalSpent = g.Where(o => o.Status != OrderStatus.Cancelled).Sum(o => o.Total)
                })
                .ToDictionaryAsync(x => x.UserId);

            return customers.Select(u =>
            {
                orderStats.TryGetValue(u.UserId, out var stat);
                return new ManagerCustomerDto
                {
                    CustomerId = u.UserId,
                    Name = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone ?? "غير مسجل",
                    TotalOrders = stat?.TotalOrders ?? 0,
                    TotalSpent = stat?.TotalSpent ?? 0m
                };
            }).ToList();
        }
    }
}