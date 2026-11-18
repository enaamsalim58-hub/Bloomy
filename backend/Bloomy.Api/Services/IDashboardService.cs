using Bloomy.Api.DTOs.Dashboard;

namespace Bloomy.Api.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<List<OrderStatusSummaryDto>> GetOrdersByStatusAsync();
        Task<List<MonthlySalesDto>> GetMonthlySalesAsync();
        Task<List<ManagerCustomerDto>> GetCustomersAsync();
    }
}