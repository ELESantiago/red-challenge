
namespace REDChallenge.Domain.Entities
{
    public class Customer : IDomainEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public IEnumerable<Order>? Orders { get; set; }
    }
}
