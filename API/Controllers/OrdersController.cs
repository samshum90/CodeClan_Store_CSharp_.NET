using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogProductActivity))]
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
        public async Task<ActionResult<IEnumerable<CustomerOrderDto>>> GetOrders()
        {
            var userId = User.GetUserId();
            
            var orders = await _unitOfWork.OrderRepository.GetOrdersByAppUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<CustomerOrderDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrderDto>> GetOrder(int id)
        {
            var userId = User.GetUserId();
            var order= await _unitOfWork.OrderRepository.GetOrderByIdAsync(id, userId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            return _mapper.Map<CustomerOrderDto>(order);
        }

        [HttpGet("basket")]
        public async Task<ActionResult<CustomerOrderDto>> GetBasket()
        {
            var userId = User.GetUserId();
            var order= await _unitOfWork.OrderRepository.GetOpenOrderByAppUserIdAsync(userId);
            if (order == null)
            {
                return Ok();
            }
            var orderDto = _mapper.Map<CustomerOrderDto>(order);
            return orderDto;
        }

        [HttpPost()]
        public async Task<ActionResult<OrderDto>> AddProduct( [FromBody] OrderedProductsDto orderedProductsDto)
        {
            var userId = User.GetUserId();
            // if (userId == null)
            // {
            //     return Unauthorized();
            // };
            var appUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var order = await _unitOfWork.OrderRepository.GetOpenOrderByAppUserIdAsync(userId);

            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(orderedProductsDto.Product.Id);

            if (product == null)
            {
                return NotFound("Item not found");
            }

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

            OrderedProducts orderedProducts = new OrderedProducts
            {
                Order = order,
                Product = product,
                Quantity = orderedProductsDto.Quantity
            };
            _unitOfWork.OrderRepository.CreateOrderedProducts( orderedProducts);
            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<CustomerOrderDto>(order));

            return BadRequest("Failed to create basket");
        }

        [HttpPost("update")]
        public async Task<ActionResult<OrderDto>> updateOrder( [FromBody] LocalStorageOrderDto orderDto)
        {
            var userId = User.GetUserId();
 
            var appUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var order = await _unitOfWork.OrderRepository.GetOpenOrderByAppUserIdAsync(userId);
            if (order == null) return NotFound("Failed to find an open order");

            var orderedProducts = _mapper.Map<OrderedProducts>(orderDto.OrderedProducts);
            order.OrderedProducts = order.OrderedProducts.AddRange(orderedProducts);
            _mapper.Map(orderDto, order);

            _unitOfWork.OrderRepository.Update(order);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<CustomerOrderDto>(order));

            return BadRequest("Failed to update order");
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
            var order = await _unitOfWork.OrderRepository.GetOpenOrderByAppUserIdAsync(User.GetUserId());
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
            var order = await _unitOfWork.OrderRepository.GetOpenOrderByAppUserIdAsync(User.GetUserId());
            if (order == null) return NotFound("Failed to find an open order");

            var orderedProducts = await _unitOfWork.OrderRepository.GetOrderedProductsByProductIdAndOrderIdAsync(itemId, order.Id);
            if (orderedProducts == null) return NotFound("Failed to find an ordered product");

            _mapper.Map(quantityDto, orderedProducts );

            if (await _unitOfWork.Complete()) return NoContent();
            
            return BadRequest("Failed to update products in order");

        }

        [HttpPut("complete/{id}")]
         public async Task<ActionResult> CompleteOrder( int id)
        {
            var order = await _unitOfWork.OrderRepository.GetOpenOrderByAppUserIdAsync(User.GetUserId());
            if (order == null) return NotFound("Failed to find an open order");

            order.OrderDate = DateTime.Now;
            order.Status = "Ordered";

            if (await _unitOfWork.Complete()) return NoContent();
            
            return BadRequest("Failed to update products in order");

        }
    }

}