using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Exceptions;
using REDChallenge.Domain.Repository;

namespace REDChallenge.Persistance.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly REDChallengeContext _context;
        public OrderRepository(REDChallengeContext context)
        {
            _context = context;
        }

        public async Task<Order> GetById(Guid id)
        {
            try
            {
                return await _context.Order
                .Include(o => o.OrderType)
                .Include(o => o.CreatedBy)
                .Include(o => o.Customer)
                .SingleAsync(o => o.Id == id);
            } catch (InvalidOperationException ex)
            {
                if (ex.Message == "Sequence contains no elements.")
                {
                    throw new NotFoundException($"Order {id} not found");
                }
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Order
                .Include(o => o.OrderType)
                .Include(o => o.CreatedBy)
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order toCreate)
        {
            await _context.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            await _context.Entry(toCreate).Reference(o => o.OrderType).LoadAsync();
            await _context.Entry(toCreate).Reference(o => o.Customer).LoadAsync();
            await _context.Entry(toCreate).Reference(o => o.CreatedBy).LoadAsync();

            return toCreate;
        }

        public async Task<bool> DeleteAsync(Order toDelete)
        {
            var order = await _context.Order.SingleAsync(o => o.Id == toDelete.Id);
            order.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> SearchOrderAsync(int? orderTypeId, string customerName, string createdByName)
        {
            var predicate = _context.Order
                .Where(x => x.IsDeleted == false);

            if (orderTypeId != null)
                predicate = predicate.Where(x => x.OrderTypeId == orderTypeId);

            if (!string.IsNullOrWhiteSpace(customerName))
                predicate = predicate.Where(x => x.Customer!.Name.Contains(customerName));

            if (!string.IsNullOrWhiteSpace(createdByName))
                predicate = predicate.Where(x => x.CreatedBy!.Name.Contains(createdByName));
            
            return await predicate
                .Include(o => o.OrderType)
                .Include(o => o.CreatedBy)
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public async Task<Order> UpdateAsync(Order toUpdate)
        {
            var order = await _context.Order.SingleAsync(o => o.Id == toUpdate.Id);
            order.OrderTypeId = toUpdate.OrderTypeId;
            order.CustomerId = toUpdate.CustomerId;
            await _context.SaveChangesAsync();

            await _context.Entry(order).Reference(o => o.OrderType).LoadAsync();
            await _context.Entry(order).Reference(o => o.Customer).LoadAsync();
            await _context.Entry(order).Reference(o => o.CreatedBy).LoadAsync();
            return order;
        }
    }
}
