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
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<OrderDto> GetOrderDtoByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByAppUserIdAsync(int appUserId);
        Task<IEnumerable<Order>> GetOrderByAppUserIdAsync(int appUserId);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
        void CreateOrderedProducts(OrderedProducts orderedProducts);
    }
}