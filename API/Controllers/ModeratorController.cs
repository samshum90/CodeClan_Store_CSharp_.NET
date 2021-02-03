using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireModeratorRole")]
    public class ModeratorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public ModeratorController(IMapper mapper, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }
        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("order/{id}")]
        public async Task<ActionResult<AdminOrderDto>> GetOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetAdminOrderDtoByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpGet("product/{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [HttpPut("product/{id}")]
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
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var productCheck = await _unitOfWork.ProductRepository.GetProductByNameAsync(product.Name);

            if (productCheck != null)return BadRequest("Product name is taken");
            _unitOfWork.ProductRepository.AddProduct(product);
            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, _mapper.Map<ProductDto>(product));

            return BadRequest("Failed to add product");
        }

        [HttpDelete("product/{id}")]
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

        [HttpPost("product/add-photo/{itemId}")]
        public async Task<ActionResult<ProductPhotoDto>> AddPhoto(int itemId, IFormFile file)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(itemId);

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new ProductPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (product.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            product.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetProduct", new { id = itemId }, _mapper.Map<ProductPhotoDto>(photo));
            }


            return BadRequest("Problem addding photo");
        }
        [HttpPut("product/set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByPhotoIdAsync(photoId);
            if (product == null)
            {
                return NotFound();
            }
            var photo = product.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = product.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("product/delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByPhotoIdAsync(photoId);
            if (product == null)
            {
                return NotFound();
            }
            
            var photo = product.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            product.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}