using Microsoft.Extensions.Logging;
using Moq;
using REDChallenge.Application.Services;
using REDChallenge.Application.Models;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Repository;
using OrderType = REDChallenge.Domain.Entities.OrderType;
using Castle.Core.Resource;
using System.Globalization;

namespace Application.UnitTest
{
    public class OrderServiceTests
    {
        private Guid userGuid = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private Guid customerGuid = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private Guid orderId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        private DateTime testDateTime = DateTime.ParseExact("2022-11-11 12:00", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        private ILogger<OrderService> mockLoggerObj = Mock.Of<ILogger<OrderService>>();
        private Order mockOrder = new Order();
        private User mockUser = new User();
        private Customer mockCustomer = new Customer();
        private OrderType mockOrderType = new OrderType();

        [SetUp]
        public void Setup()
        {
            mockUser = new User { Id = userGuid, Name = "Test user" };
            mockCustomer = new Customer { Id = customerGuid, Name = "Test user" };
            mockOrderType = new OrderType { Id = 1, Name = "Test Order Type" };

            mockOrder = new Order
            {
                Id = orderId,
                CreatedById = userGuid,
                IsDeleted = false,
                OrderTypeId = 1,
                CustomerId = customerGuid,
                CreatedDate = testDateTime,
                CreatedBy = mockUser,
                Customer = mockCustomer,
                OrderType = mockOrderType,
            };
        }

        [Test]
        public async Task CreateOrderTest()
        {
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(m => m.CreateAsync(It.IsAny<Order>())).ReturnsAsync(mockOrder);

            var orderService = new OrderService(mockRepo.Object, mockLoggerObj);
            var newOrder = await orderService.CreateOrder(new CreateOrderModel
            {
                CustomerId = customerGuid,
                OrderTypeId = 1
            }, userGuid);
            var expected = new OrderModel
            {
                Id = orderId,
                CreatedById = userGuid,
                CreatedByUsername = mockUser.Name,
                CreatedDate = testDateTime,
                CustomerId = customerGuid,
                CustomerName = mockCustomer.Name,
                TypeId = mockOrderType.Id,
                Type = mockOrderType.Name
            };
            Assert.IsTrue(OrderModelAreEqual(expected, newOrder));
            Assert.Pass();
        }

        [Test]
        public async Task DeleteOrderTest()
        {
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(m => m.DeleteAsync(It.IsAny<Order>())).ReturnsAsync(true);

            var orderService = new OrderService(mockRepo.Object, mockLoggerObj);
            await orderService.DeleteOrder(orderId);
            Assert.Pass();
        }

        [Test]
        public async Task GetOrderByIdTest()
        {
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(m => m.GetById(It.IsAny<Guid>())).ReturnsAsync(mockOrder);

            var orderService = new OrderService(mockRepo.Object, mockLoggerObj);
            var newOrder = await orderService.GetOrderById(orderId);
            var expected = new OrderModel
            {
                Id = orderId,
                CreatedById = userGuid,
                CreatedByUsername = mockUser.Name,
                CreatedDate = testDateTime,
                CustomerId = customerGuid,
                CustomerName = mockCustomer.Name,
                TypeId = mockOrderType.Id,
                Type = mockOrderType.Name
            };
            Assert.IsTrue(OrderModelAreEqual(expected, newOrder));
            Assert.Pass();
        }

        [Test]
        public async Task UpdatedOrderTest()
        {
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(m => m.UpdateAsync(It.IsAny<Order>())).ReturnsAsync(mockOrder);

            var orderService = new OrderService(mockRepo.Object, mockLoggerObj);
            var newOrder = await orderService.UpdateOrder(new UpdateOrderModel
            {
                CustomerId = customerGuid,
                OrderTypeId = 1
            });

            var expected = new OrderModel
            {
                Id = orderId,
                CreatedById = userGuid,
                CreatedByUsername = mockUser.Name,
                CreatedDate = testDateTime,
                CustomerId = customerGuid,
                CustomerName = mockCustomer.Name,
                TypeId = mockOrderType.Id,
                Type = mockOrderType.Name
            };
            Assert.IsTrue(OrderModelAreEqual(expected, newOrder));
            Assert.Pass();
        }

        private bool OrderModelAreEqual(OrderModel? orderModel1, OrderModel? orderModel2)
        {
            if (orderModel1 == null || orderModel2 == null)
                return false;
            return orderModel1.Id == orderModel2.Id &&
                orderModel1.Type == orderModel2.Type &&
                orderModel1.CustomerName == orderModel2.CustomerName &&
                orderModel1.CustomerId == orderModel2.CustomerId &&
                orderModel1.CreatedDate == orderModel2.CreatedDate &&
                orderModel1.CreatedByUsername == orderModel2.CreatedByUsername &&
                orderModel1.CreatedById == orderModel2.CreatedById;
        }
    }
}