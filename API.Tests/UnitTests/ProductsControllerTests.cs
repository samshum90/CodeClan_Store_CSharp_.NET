using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.UnitTests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProduct_ReturnProduct()
        {
            // Arrange
            string testName = "Test One";
            var mockRepo = new Mock<IProductRepository>();
            var mockUoW = new Mock<IUnitOfWork>();

            mockUoW.Setup(x => x.ProductRepository).Returns(mockRepo.Object);
                    mockUoW.Setup(x => x.ProductRepository.GetProductByNameAsync(testName))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Name == testName));

                    var mockPhotoService = new Mock<IPhotoService>();
            
                
            var mockMapper = new Mock<IMapper>();
            var controller = new ProductsController( mockMapper.Object, mockUoW.Object, mockPhotoService.Object);

            // Act
            var result = await controller.GetProduct(testName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Product>>(okResult.Value);
            var product = returnValue.FirstOrDefault();
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