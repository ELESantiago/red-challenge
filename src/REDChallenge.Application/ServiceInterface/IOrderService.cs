using REDChallenge.Application.Models;

namespace REDChallenge.Application.ServiceInterface
{
    public interface IOrderService
    {
        Task<OrderModel> GetOrderById(Guid orderId);
        Task<OrderModel> CreateOrder(CreateOrderModel order, Guid userId);
        Task<OrderModel> UpdateOrder(UpdateOrderModel order);
        Task DeleteOrder(Guid orderId);
        Task<IEnumerable<OrderModel>> SearchOrders(SearchOrderModel searchParams);
    }
}
