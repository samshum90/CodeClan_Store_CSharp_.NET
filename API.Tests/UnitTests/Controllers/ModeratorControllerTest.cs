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