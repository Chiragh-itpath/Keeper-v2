using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class ContactRepo : IContactRepo
    {
        private readonly DbKeeperContext _db;
        public ContactRepo(DbKeeperContext db)
        {
            _db = db;
        }
        public async Task<ContactModel> AddAsync(ContactModel contact)
        {
            await _db.AddAsync(contact);
            await _db.SaveChangesAsync();
            return contact;
        }
        public async Task<List<ContactModel>> GetAllAsync(Guid userId)
        {
            return await _db.Contact
                .Include(x => x.AddedPerson)
                .Include(x => x.AddedBy)
                .AsNoTracking()
                .Where(x => x.AddedById == userId)
                .ToListAsync();
        }
        public async Task<ContactModel> GetByIdAsync(Guid id)
        {
            return await _db.Contact
                .Include(x => x.AddedBy)
                .Include(x => x.AddedPerson)
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);
        }
    }
}
