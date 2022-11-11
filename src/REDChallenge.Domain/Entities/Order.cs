
namespace REDChallenge.Domain.Entities
{
    public class Order : IDomainEntity<Guid>
    {
        public Guid Id { get; set; }
        public int OrderTypeId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Customer? Customer { get; set; }
        public User? CreatedBy { get; set; }
        public OrderType? OrderType { get; set; }
    }
}
