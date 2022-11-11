
namespace REDChallenge.Domain.Entities
{
    public interface IDomainEntity<TId>
    {
        TId Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
