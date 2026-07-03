using StorePro.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorePro.Core.Services
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto);
        Task<bool> CancelOrderAsync(Guid orderId);
        Task<OrderPrintDto> GetOrderForPrintAsync(Guid orderId);
    }
}