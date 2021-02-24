using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync();

            return Ok( _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products));
        }

        [HttpGet("{productname}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string productname)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByNameAsync(productname);

            if (product == null)
            {
                return NotFound(productname);
            }

            return _mapper.Map<ProductDto>(product);
        }
    }
}