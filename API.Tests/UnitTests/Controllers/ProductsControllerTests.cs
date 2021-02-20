using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.UnitTests
{
    public class ProductsControllerTests
    {

        private readonly Mock<IUnitOfWork> _iUnitOfWork;
        private readonly Mock<IProductRepository> _iProductRepo;
        private readonly Mock<IPhotoService> _iPhotoService;
        private readonly IMapper _iMapper;

        public ProductsControllerTests()
        {
            _iProductRepo = new Mock<IProductRepository>();
            _iUnitOfWork = new Mock<IUnitOfWork>();

            var autoMapperProfiles = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMapperProfiles));
            _iMapper = new Mapper(configuration);

            _iUnitOfWork.Setup(x => x.ProductRepository).Returns(_iProductRepo.Object);
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductsAsync())
                .ReturnsAsync(GetTestProducts());
        }

        [Fact]
        public async Task GetProducts_ReturnOKResult()
        {
            // Arrange
            var controller = new ProductsController( _iMapper, _iUnitOfWork.Object);

            // Act
            var okResult = await controller.GetProducts();

            // Assert
            var returnResult = Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetProducts_ReturnAllProducts()
        {
            // Arrange
            var controller = new ProductsController( _iMapper, _iUnitOfWork.Object);

            // Act
            var result = await controller.GetProducts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductDto>>>(result);
            var returnValue = actionResult.Result as OkObjectResult;
            var products = returnValue.Value as IEnumerable<ProductDto>;
            var product = products.FirstOrDefault(p => p.Name == "Test One");
            Assert.Equal("Test One", product.Name);
            Assert.Equal(2, products.Count());

        }

        [Fact]
        public async Task GetProduct_ReturnsNotFoundObjectResultForNonexistentProduct()
        {
            // Arrange
            string nonExistentTestName = "Test Four";

            var controller = new ProductsController( _iMapper, _iUnitOfWork.Object);
            // Act
            var result = await controller.GetProduct(nonExistentTestName);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductDto>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetProduct_ReturnProduct()
        {
            // Arrange
            string testName = "Test One";
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByNameAsync(testName))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Name == testName));

            var controller = new ProductsController( _iMapper, _iUnitOfWork.Object);
            // Act
            var result = await controller.GetProduct(testName);

            // Assert
            var okResult = Assert.IsType<ActionResult<ProductDto>>(result);
            var product = Assert.IsType<ProductDto>(result.Value);
            Assert.Equal("Test One", product.Name);
        }

        private List<Product> GetTestProducts()
        {
            var products = new List<Product>();
            products.Add(new Product()
            {
                Id = 1,
                Name = "Test One",
                ProductPrice = "1.00",
                SalePrice = "2.00",
                Description ="Test One Description",
                Category = "One",
                Stock = 1,
                Highlight = true,
            });
            products.Add(new Product()
            {
                Id = 2,
                Name = "Test Two",
                ProductPrice = "4.00",
                SalePrice = "2.00",
                Description ="Test Two Description",
                Category = "Two",
                Stock = 2,
                Highlight = false,
            });
            return products;
        }
    
    }
}