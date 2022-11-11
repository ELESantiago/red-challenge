
using Microsoft.EntityFrameworkCore;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Repository;

namespace REDChallenge.Persistance.Repository
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        private readonly REDChallengeContext _context;
        public UserRepository(REDChallengeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.User.SingleOrDefaultAsync(u => u.Name == username);
        }
    }
}
