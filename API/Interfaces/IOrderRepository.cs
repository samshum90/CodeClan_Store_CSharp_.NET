using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IOrderRepository
    {
        void Update(Order order);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrderByAppUserIdAsync(int appUserId);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
    }
}