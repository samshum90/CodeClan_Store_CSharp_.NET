using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var userId = User.GetUserId();
            
            var orders = await _unitOfWork.OrderRepository.GetOrdersByAppUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetProduct(int id)
        {
 
            var order= await _unitOfWork.OrderRepository.GetOrderDtoByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost()]
        public async Task<ActionResult<OrderDto>> AddProduct([FromBody] ProductDto productDto)
        {
            var userId = User.GetUserId();
            var appUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var orders = await _unitOfWork.OrderRepository.GetOrderByAppUserIdAsync(userId);

            Order order = null;

            order = orders.SingleOrDefault(o => o.Status == "Open");

            if (order == null)
            {
                order = new Order
                    {
                        Status = "Open",
                        AppUser = appUser,
                        AppUserId = userId,
                    };
                 _unitOfWork.OrderRepository.CreateOrder(order);
            }
            var product = await _unitOfWork.ProductRepository.GetProductByNameAsync(productDto.Name);

            if (product == null)
            {
                return NotFound();
            }

            OrderedProducts orderedProducts = new OrderedProducts
            {
                Order = order,
                Product = product,
                Quantity = 1
            };
            _unitOfWork.OrderRepository.CreateOrderedProducts( orderedProducts);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to create basket");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderRepository.DeleteOrder(order);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to Delete order");
        }

        [HttpDelete("delete-item/{itemId}")]
        public async Task<ActionResult> DeleteItemFromOrder(int itemId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrderByAppUserIdAsync(User.GetUserId());
            var order = orders.SingleOrDefault(o => o.Status == "Open");
            if (order == null) return NotFound("Failed to find an open order");

            var item = order.OrderedProducts.SingleOrDefault(p => p.ProductId == itemId);
            if (item == null) return NotFound("Failed to find item in order");

            order.OrderedProducts.Remove(item);

            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to remove the item from order");
        }

        [HttpPut("edit-item/{itemId}")]
        public async Task<ActionResult> UpdateProductsInOrder( int itemId, [FromBody] QuantityDto quantityDto)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrderByAppUserIdAsync(User.GetUserId());
            var order = orders.SingleOrDefault(o => o.Status == "Open");
            if (order == null) return NotFound("Failed to find an open order");

            var orderedProducts = await _unitOfWork.OrderRepository.GetOrderedProductsByProductIdAndOrderIdAsync(itemId, order.Id);
             if (orderedProducts == null) return NotFound("Failed to find an ordered product");

            _mapper.Map(quantityDto, orderedProducts );


            if (await _unitOfWork.Complete()) return NoContent();
            
            return BadRequest("Failed to update products in order");
            
        }
    }

}