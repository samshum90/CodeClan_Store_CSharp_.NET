using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetOrderByAppUserIdAsync(int appUserId)
        {
            return await _context.Orders
                .Where( o => o.AppUserId == appUserId)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .ToListAsync();
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified; }
    }
}