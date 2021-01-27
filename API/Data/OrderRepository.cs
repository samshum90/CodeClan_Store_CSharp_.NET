using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public void CreateOrderedProducts(OrderedProducts orderedProducts)
        {
             _context.OrderedProducts.Add(orderedProducts);
        }
        public async Task<OrderedProducts> GetOrderedProductsByProductIdAsync(int productId)
        {
            return await _context.OrderedProducts.SingleOrDefaultAsync(x => x.ProductId == productId);
        }
        public async Task<OrderedProducts> GetOrderedProductsByProductIdAndOrderIdAsync(int productId, int orderId)
        {
            return await _context.OrderedProducts.SingleOrDefaultAsync(x => x.ProductId == productId && x.OrderId == orderId);
        }

        public void CreateOrder(Order order)
        {
             _context.Orders.Add(order);
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order;
        }

        public async Task<OrderDto> GetOrderDtoByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderedProducts)
                .ThenInclude(p => p.Product)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == id);
            return order;
        }
        public async Task<AdminOrderDto> GetAdminOrderDtoByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderedProducts)
                .ThenInclude(p => p.Product)
                .ProjectTo<AdminOrderDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public async Task<IEnumerable<AdminOrderDto>> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .ProjectTo<AdminOrderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return orders;
        }
        public async Task<IEnumerable<OrderDto>> GetOrdersByAppUserIdAsync(int appUserId)
        {
            var orders = await _context.Orders
                .Where(o => o.AppUserId == appUserId)
                .Include(o => o.OrderedProducts)
                .ThenInclude(p => p.Product)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return orders;

        }
        public async Task<IEnumerable<Order>> GetOrderByAppUserIdAsync(int appUserId)
        {
            var order = await _context.Orders
                .Where(o => o.AppUserId == appUserId)
                .Include(o => o.OrderedProducts)
                .ToListAsync();

            return order;

        }
        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }
        public void UpdateOrderedProducts (OrderedProducts  orderedProducts )
        {
            _context.Entry(orderedProducts).State = EntityState.Modified;
        }

    }
}