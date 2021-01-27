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
        private readonly IPhotoService _photoService;
        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{productname}")]
        public async Task<ActionResult<Product>> GetProduct(string productname)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByNameAsync(productname);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}