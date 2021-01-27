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
    [Route("api/")]
    [ApiController]
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

        [HttpGet("products/")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("product/{productname}", Name = "GetProduct")]
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
                return CreatedAtRoute("GetProduct", new { productname = product.Name }, _mapper.Map<ProductPhotoDto>(photo));
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