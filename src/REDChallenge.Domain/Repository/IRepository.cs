using REDChallenge.Domain.Entities;

namespace REDChallenge.Domain.Repository
{
    public interface IRepository<T, TId> where T : IDomainEntity<TId>
    {
        Task<T> GetById(TId id);
        Task<IEnumerable<T>> GetAll();
        Task<T> CreateAsync(T toCreate);
        Task<T> UpdateAsync(T toUpdate);
        Task<bool> DeleteAsync(T toDelete);
    }
}
