
namespace REDChallenge.Domain.Entities
{
    public class User : IDomainEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

        public IEnumerable<Order>? Orders { get; set; }
    }
}
