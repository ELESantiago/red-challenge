using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Exceptions;
using REDChallenge.Domain.Repository;

namespace REDChallenge.Persistance.Repository
{
    public class GenericRepository<T, TId> : IRepository<T, TId> where T : class, IDomainEntity<TId>
    {

        private readonly REDChallengeContext _context;
        public GenericRepository(REDChallengeContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T toCreate)
        {
            await _context.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return toCreate;
        }

        public async Task<bool> DeleteAsync(T toDelete)
        {
            var record = await GetById(toDelete.Id);
            record.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(TId id)
        {
            var record = await _context.Set<T>().SingleAsync(o => o.Id!.Equals(id));
            if (record == null)
                throw new NotFoundException();
            return record;
        }

        public async Task<T> UpdateAsync(T toUpdate)
        {
            _context.Attach(toUpdate);
            await _context.SaveChangesAsync();
            return toUpdate;
        }

    }
}
