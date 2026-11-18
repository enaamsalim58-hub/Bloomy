using Bloomy.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bloomy.Api.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _dashboardService.GetStatsAsync();
            return Ok(stats);
        }

        [HttpGet("orders-by-status")]
        public async Task<IActionResult> GetOrdersByStatus()
        {
            var summary = await _dashboardService.GetOrdersByStatusAsync();
            return Ok(summary);
        }

        [HttpGet("sales-monthly")]
        public async Task<IActionResult> GetMonthlySales()
        {
            var monthly = await _dashboardService.GetMonthlySalesAsync();
            return Ok(monthly);
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _dashboardService.GetCustomersAsync();
            return Ok(customers);
        }
    }
}