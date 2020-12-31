using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpGet("products/")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("product/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("product/{id}")]
        public async Task<ActionResult> UpdateProduct( int id, [FromForm] ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return  NotFound();
            }

            _mapper.Map(productDto, product);
            _context.Entry(product).State = EntityState.Modified;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
            
        }

        [HttpPost("product")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}