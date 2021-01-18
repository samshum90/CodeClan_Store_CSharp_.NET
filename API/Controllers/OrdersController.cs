using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetProduct(int id)
        {
 
            var order= await _unitOfWork.OrderRepository.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost()]
        public async Task<ActionResult<OrderDto>> CreateBasket(BasketDto basket)
        {
            var userId = User.GetUserId();
            var appUser = _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            
            order = new Order
            {

            };

            _unitOfWork.OrderRepository.CreateOrder(basket);
            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);

            return BadRequest("Failed to create basket");
        }
    }

}