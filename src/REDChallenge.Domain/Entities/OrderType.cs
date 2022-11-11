
namespace REDChallenge.Domain.Entities
{
    public class OrderType : IDomainEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
