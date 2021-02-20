using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        void Update(Order order);
        void DeleteOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOpenOrderByAppUserIdAsync(int appUserId);
        Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersByAppUserIdAsync(int appUserId);
        Task<CustomerOrderDto> GetCustomerOrderDtoByIdAsync(int id, int userId);
        // Task<IEnumerable<AdminOrderDto>> GetAdminOrdersAsync();
        // Task<AdminOrderDto> GetAdminOrderDtoByIdAsync(int id);
        void CreateOrderedProducts(OrderedProducts orderedProducts);
        void UpdateOrderedProducts(OrderedProducts orderedProducts);
        Task<OrderedProducts> GetOrderedProductsByProductIdAsync(int productId);
        Task<OrderedProducts> GetOrderedProductsByProductIdAndOrderIdAsync(int productId, int orderId);
    }
}