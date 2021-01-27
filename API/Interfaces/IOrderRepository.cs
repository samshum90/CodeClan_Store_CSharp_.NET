using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        void Update(Order order);
        Task<OrderedProducts> GetOrderedProductsByProductIdAsync(int productId);
        Task<OrderedProducts> GetOrderedProductsByProductIdAndOrderIdAsync(int productId, int orderId);
        Task<AdminOrderDto> GetAdminOrderDtoByIdAsync(int id);
        Task<IEnumerable<AdminOrderDto>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderDto> GetOrderDtoByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByAppUserIdAsync(int appUserId);
        Task<IEnumerable<Order>> GetOrderByAppUserIdAsync(int appUserId);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
        void CreateOrderedProducts(OrderedProducts orderedProducts);
    }
}