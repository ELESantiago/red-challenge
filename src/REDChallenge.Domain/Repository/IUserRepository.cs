using REDChallenge.Domain.Entities;

namespace REDChallenge.Domain.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetUserByUsername(string username);
    }
}
