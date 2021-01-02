using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("products/")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("product/{productname}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetProduct(string productname)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByNameAsync(productname);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("product/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateProduct( int id, [FromForm] ProductDto productDto)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return  NotFound();
            }

            _mapper.Map(productDto, product);

            _unitOfWork.ProductRepository.Update(product);

            if (await _unitOfWork.Complete()) return NoContent();
            
            return BadRequest("Failed to update product");
            
        }

        [HttpPost("product")]
        [Authorize]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {

            _unitOfWork.ProductRepository.AddProduct(product);
            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);

            return BadRequest("Failed to add product");
        }

        [HttpDelete("product/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.DeleteProduct(product);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to Delete product");
        }
    }
}