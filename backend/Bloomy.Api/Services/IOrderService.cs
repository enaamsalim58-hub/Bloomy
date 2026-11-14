using Bloomy.Api.DTOs;
//using Bloomy.Api.DTOs.Orders;
using Bloomy.Api.Models;

namespace Bloomy.Api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO?> GetOrderAsync(int orderId);

        Task<List<OrderResponseDTO>> GetUserOrdersAsync(int userId);

        Task<bool> UpdateOrderStatusAsync(
            int orderId,
            UpdateOrderStatusDTO dto);

        Task<OrderResponseDTO?> CreateOrderAsync(int userId, CreateOrderDTO dto);
    }
}