using FluentValidation;
using Microsoft.Extensions.Logging;
using REDChallenge.Application.Models;
using REDChallenge.Application.ServiceInterface;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Repository;

namespace REDChallenge.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository repository, ILogger<OrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OrderModel> CreateOrder(CreateOrderModel order, Guid userId)
        {
            _logger.LogInformation("User {userId} is creating a new order", userId);
            var dbOrder = new Order
            {
                CreatedById = userId,
                CustomerId = order.CustomerId,
                OrderTypeId = order.OrderTypeId,
            };
            dbOrder = await _repository.CreateAsync(dbOrder);

            return new OrderModel
            {
                Id = dbOrder.Id,
                CreatedDate = dbOrder.CreatedDate,
                TypeId = dbOrder.OrderTypeId,
                Type = dbOrder.OrderType!.Name,
                CreatedById = dbOrder.CreatedById,
                CreatedByUsername = dbOrder.CreatedBy!.Name,
                CustomerId = dbOrder.CustomerId,
                CustomerName = dbOrder.Customer!.Name,                
            };
        }

        public async Task DeleteOrder(Guid orderId)
        {
            _logger.LogInformation("Deleting order {orderId}", orderId);
            await _repository.DeleteAsync(new Order { Id = orderId });
            _logger.LogInformation("Deleted order {orderId}", orderId);
        }

        public async Task<OrderModel> GetOrderById(Guid orderId)
        {
            _logger.LogInformation("Trying to get order by id {orderId}", orderId);
            try
            {
                var order = await _repository.GetById(orderId);

                return new OrderModel
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CreatedByUsername = order.CreatedBy!.Name,
                    CustomerName = order.Customer!.Name,
                    CreatedById = order.CreatedById,
                    CreatedDate = order.CreatedDate,
                    TypeId = order.OrderTypeId,
                    Type = order.OrderType!.Name,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception happen while trying to fetch order with id {orderId}: Exception {ex}", orderId, ex);
                throw;
            }
            
        }

        public async Task<OrderModel> UpdateOrder(UpdateOrderModel order)
        {
            _logger.LogInformation("Updating order {orderId}", order.Id);
            var dbOrder = await _repository.UpdateAsync(new Order
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderTypeId = order.OrderTypeId,
            });
            _logger.LogInformation("Updated order {orderId}", order.Id);

            return new OrderModel
            {
                Id = dbOrder.Id,
                CreatedById = dbOrder.CreatedById,
                CreatedByUsername = dbOrder.CreatedBy!.Name,
                CustomerId = dbOrder.CustomerId,
                CustomerName = dbOrder.Customer!.Name,
                CreatedDate = dbOrder.CreatedDate,
                TypeId = dbOrder.OrderTypeId,
                Type = dbOrder.OrderType!.Name,
            };
        }

        public async Task<IEnumerable<OrderModel>> SearchOrders(SearchOrderModel searchParams)
        {
            _logger.LogInformation("Searching for orders with params orderType {0} - createdBy {1} - customer {2}", 
                searchParams.OrderType, searchParams.CreatedBy, searchParams.CustomerName);
            var orders = await _repository.SearchOrderAsync(searchParams.OrderType, searchParams.CustomerName, searchParams.CreatedBy);
            _logger.LogInformation("{0} orders found that match", orders.Count());
            return orders.Select(o => new OrderModel
            {
                Id = o.Id,
                CreatedById = o.CreatedById,
                CustomerId = o.CustomerId,
                CreatedByUsername = o.CreatedBy!.Name,
                CreatedDate = o.CreatedDate,
                CustomerName = o.Customer!.Name,
                TypeId = o.OrderTypeId,
                Type = o.OrderType!.Name,
            });
        }
    }
}
