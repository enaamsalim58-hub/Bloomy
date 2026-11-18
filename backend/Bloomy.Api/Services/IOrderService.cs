using Bloomy.Api.DTOs;
using Bloomy.Api.Models;

namespace Bloomy.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDTO>> GetAllOrdersAsync();
        Task<OrderResponseDTO?> GetOrderAsync(int orderId);
        Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId, OrderStatus? status = null);
        Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDTO dto);
        Task<(OrderResponseDTO? Order, string? Error, bool NotFound)> CreateOrderAsync(int userId, CreateOrderDTO dto);
    }
}