namespace Bloomy.Api.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int AvailableProducts { get; set; }
    }

    public class OrderStatusSummaryDto
    {
        public string Status { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class MonthlySalesDto
    {
        public int MonthNumber { get; set; }
        public string Month { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Percent { get; set; }
    }

    public class ManagerCustomerDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}