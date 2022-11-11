using REDChallenge.Domain.Entities;

namespace REDChallenge.Domain.Repository
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> SearchOrderAsync(int? orderTypeId, string customerName, string createdByName);
    }
}
