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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.UnitTests.Controllers
{
    public class ModeratorControllerTest
    {
        private readonly Mock<IUnitOfWork> _iUnitOfWork;
        private readonly Mock<IProductRepository> _iProductRepo;
        private readonly Mock<IOrderRepository> _iOrderRepo;
        private readonly Mock<IPhotoService> _iPhotoService;
        private readonly IMapper _iMapper;

        public ModeratorControllerTest()
        {
            _iPhotoService = new Mock<IPhotoService>();
            _iProductRepo = new Mock<IProductRepository>();
            _iUnitOfWork = new Mock<IUnitOfWork>();
            _iOrderRepo = new Mock<IOrderRepository>();

            var autoMapperProfiles = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(autoMapperProfiles));
            _iMapper = new Mapper(configuration);
           
        //    _iMapper.Setup(m => m.Map<Order, AdminOrderDto>(It.IsAny<Order>())).Returns(new AdminOrderDto());
            _iUnitOfWork.Setup(x => x.ProductRepository).Returns(_iProductRepo.Object);
            _iUnitOfWork.Setup(x => x.OrderRepository).Returns(_iOrderRepo.Object);
            _iUnitOfWork.Setup(x => x.OrderRepository.GetOrdersAsync())
                .ReturnsAsync(GetTestOrders());
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductsAsync())
                .ReturnsAsync(GetTestProducts());
            
        }

        [Fact]
        public async Task GetOrders_ReturnOKResult()
        {
            // Arrange
            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var okResult = await controller.GetOrders();

            // Assert
            var returnResult = Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetOrders_ReturnAllOrders()
        {
            // Arrange
            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.GetOrders();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<AdminOrderDto>>>(result);
            var returnValue = actionResult.Result as OkObjectResult;
            var orders = returnValue.Value as IEnumerable<AdminOrderDto>;
            Assert.Equal(1, orders.Count());
            var order = orders.FirstOrDefault(p => p.Id == 1);
            Assert.Equal(1, order.Id);
            
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFoundObjectResultForNonexistentProduct()
        {
             // Arrange
            int nonExistentId = 999;

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.GetOrder(nonExistentId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AdminOrderDto>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetOrder_ReturnOrder()
        {
            // Arrange
            int testId = 1;
            _iUnitOfWork.Setup(x => x.OrderRepository.GetOrderByIdAsync(testId))
                    .ReturnsAsync(GetTestOrders().FirstOrDefault(
                            p => p.Id == testId));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.GetOrder(testId);

            // Assert
            var okResult = Assert.IsType<ActionResult<AdminOrderDto>>(result);
            var order = Assert.IsType<AdminOrderDto>(result.Value);
            Assert.Equal(1, order.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFoundObjectResultForNonexistentProduct()
        {
            // Arrange
            int nonExistentTestId = 999;
             _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(nonExistentTestId))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Id == nonExistentTestId));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.GetProduct(nonExistentTestId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductDto>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetProduct_ReturnProduct()
        {
            // Arrange
            int testId = 1;
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(testId))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Id == testId));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.GetProduct(testId);

            // Assert
            var okResult = Assert.IsType<ActionResult<ProductDto>>(result);
            var product = Assert.IsType<ProductDto>(result.Value);
            Assert.Equal(1, product.Id);
        }

        [Fact]
        public async Task CreateProduct_ReturnBadRequest()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test One"
            };
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByNameAsync(product.Name))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Name == product.Name));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.CreateProduct(product);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var badResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Product name is taken", badResult.Value);
        }       

        [Fact]
        public async Task CreateProduct_ReturnCreateAtAction()
        {
            // Arrange
            var product = new Product
            {
                Id = 3,
                Name = "Test Three",
                ProductPrice = "2.00",
                SalePrice = "8.00",
                Description = "Test Three Description",
                Category = "Three",
                Stock = 3,
                Highlight = true,
            };
            _iUnitOfWork.Setup(x => x.ProductRepository.AddProduct(product));
            _iUnitOfWork.Setup(x => x.Complete()).Returns(() => Task.Run(() => true));

            var controller = new ModeratorController(_iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.CreateProduct(product);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateProduct_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var product = new Product
            {
                Id = 3,
                Name = "Test Three",
                ProductPrice = "2.00",
                SalePrice = "8.00",
                Description = "Test Three Description",
                Category = "Three",
                Stock = 3,
                Highlight = true,
            };
            _iUnitOfWork.Setup(x => x.ProductRepository.AddProduct(product));
            _iUnitOfWork.Setup(x => x.Complete()).Returns(() => Task.Run(() => true));

            var controller = new ModeratorController(_iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.CreateProduct(product);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var createdResult = actionResult.Result as CreatedAtActionResult;
            var returnedProduct = createdResult.Value as ProductDto;
            Assert.Equal("Test Three", returnedProduct.Name);

        }
        [Fact]
        public async Task CreateProduct_ReturnedBadResponse()
        {
            // Arrange
            var product = new Product
            {
                Id = 3,
                Name = "Test Three",
                ProductPrice = "2.00",
                SalePrice = "8.00",
                Description = "Test Three Description",
                Category = "Three",
                Stock = 3,
                Highlight = true,
            };
            _iUnitOfWork.Setup(x => x.ProductRepository.AddProduct(product));

            var controller = new ModeratorController(_iMapper, _iUnitOfWork.Object, _iPhotoService.Object);
            // Act
            var result = await controller.CreateProduct(product);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var badResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Failed to add product", badResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFoundObjectResultForNonexistentProduct()
        {
            // Arrange
            var mockProduct = new Product { 
                Id = 999
            };
             _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockProduct.Id))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Id == mockProduct.Id));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.UpdateProduct(mockProduct);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnNoContent()
        {
            // Arrange
            var mockProduct = new Product { 
                Id = 1,
                Name = "Changed Test One"
            };
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockProduct.Id))
                .ReturnsAsync(mockProduct);
            _iUnitOfWork.Setup(x => x.ProductRepository.Update(mockProduct));
            _iUnitOfWork.Setup(x => x.Complete()).Returns(() => Task.Run(() => true));


            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.UpdateProduct(mockProduct);

            // Assert
           Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async Task UpdateProduct_ReturnBadRequest()
        {
            // Arrange
            var mockProduct = new Product { 
                Id = 1,
                Name = "Changed Test One"
            };
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockProduct.Id))
                .ReturnsAsync(mockProduct);
            _iUnitOfWork.Setup(x => x.ProductRepository.Update(mockProduct));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.UpdateProduct(mockProduct);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to update product", badResult.Value);

        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFoundObjectResultForNonexistentProduct()
        {
            // Arrange
            int NonExistentId = 999;

            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(NonExistentId))
                    .ReturnsAsync(GetTestProducts().FirstOrDefault(
                            p => p.Id == NonExistentId));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.DeleteProduct(NonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnOkResponse()
        {
            // Arrange
            int mockId = 1;
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockId))
                .ReturnsAsync(GetTestProducts().FirstOrDefault(x => x.Id == mockId));
            _iUnitOfWork.Setup(x => x.ProductRepository
                .DeleteProduct(GetTestProducts().FirstOrDefault(x => x.Id == mockId)));
            _iUnitOfWork.Setup(x => x.Complete()).Returns(() => Task.Run(() => true));

            var controller = new ModeratorController(_iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.DeleteProduct(mockId);
        
            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnBadResquest()
        {
            // Arrange
            int mockId = 1;
            _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockId))
                .ReturnsAsync(GetTestProducts().FirstOrDefault(x => x.Id == mockId));
            _iUnitOfWork.Setup(x => x.ProductRepository
                .DeleteProduct(GetTestProducts().FirstOrDefault(x => x.Id == mockId)));

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.DeleteProduct(mockId);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to Delete product", badResult.Value);

        }

        [Fact]
        public async Task AddPhoto_ReturnsNotFoundObjectResultForNonexistentProduct()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            int NonExistentId = 99;

            var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

            // Act
            var result = await controller.AddPhoto(NonExistentId, fileMock.Object);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductPhotoDto>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        //  [Fact]
        // public async Task AddPhoto_ReturnsBadRequest()
        // {
        //     // Arrange
        //     var fileMock = new Mock<IFormFile>();
        //     int mockId = 1;

        //     _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockId))
        //         .ReturnsAsync(GetTestProducts().FirstOrDefault(x => x.Id == mockId));
        //     _iPhotoService.Setup(x => x.AddPhotoAsync(fileMock.Object))
        //         .Returns(() => Task.Run(() => null));

        //     _iUnitOfWork.Setup(x => x.Complete()).Returns(() => Task.Run(() => true));
        //     var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

        //     // Act
        //     var result = await controller.AddPhoto(mockId, fileMock.Object);

        //     // Assert
        //     var actionResult = Assert.IsType<ActionResult<ProductPhotoDto>>(result);
        //     Assert.IsType<NotFoundResult>(actionResult.Result);
        // }

        // [Fact]
        // public async Task AddPhoto_ReturnsCreatedAtRoute()
        // {
        //     // Arrange
        //     var fileMock = new Mock<IFormFile>();
        //     int mockId = 1;
        //     string mockUrl = "https://i.ndtvimg.com/i/2016-11/dan-dan-noodles_620x350_61479458545.jpg";
        //     string mockPublidId = "1";

        //     _iUnitOfWork.Setup(x => x.ProductRepository.GetProductByIdAsync(mockId))
        //         .ReturnsAsync(GetTestProducts().FirstOrDefault(x => x.Id == mockId));

        //     var photo = new ProductPhoto()
        //     {
        //         Url = mockUrl,
        //         PublicId = mockPublidId
        //     };

        //     _iUnitOfWork.Setup(x => x.Complete())
        //         .Returns(() => Task.Run(() => true))
        //         .Verifiable();
        //     var controller = new ModeratorController( _iMapper, _iUnitOfWork.Object, _iPhotoService.Object);

        //     // Act
        //     var result = await controller.AddPhoto(mockId, fileMock.Object);

        //     // Assert
        //     var actionResult = Assert.IsType<ActionResult<ProductDto>>(result);
        //     var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //     var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
        //     _iUnitOfWork.Verify();
        //     Assert.Equal(1, returnValue.Photos.Count());
        //     Assert.Equal(mockUrl, returnValue.Photos.LastOrDefault().Url);
        //     Assert.Equal(true, returnValue.Photos.LastOrDefault().IsMain);
        // }

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
        private Product GetTestProduct()
        {
            return new Product()
            {
                Id = 3,
                Name = "Test Three",
                ProductPrice = "2.00",
                SalePrice = "8.00",
                Description = "Test Three Description",
                Category = "Three",
                Stock = 3,
                Highlight = true,
            };

        }
        private List<Order> GetTestOrders()
        {
            var orders = new List<Order>();
            orders.Add(new Order()
            {
                Id = 1,
                OrderCreated = new DateTime(2021, 02, 16, 14, 44, 28),
                OrderDate = new DateTime(0001, 01, 01, 00, 00, 00),
                Status = "Open",
            });

            return orders;
        }
    }
}