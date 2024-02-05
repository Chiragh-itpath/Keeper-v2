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
        public async Task<ContactModel> UpdateAsync(ContactModel contact)
        {
            _db.Entry(contact).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return contact;
        }
        public async Task DeleteAsync(ContactModel contact)
        {
            _db.Remove(contact);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ContactModel>> GetAllAsync(Guid userId)
        {
            return await _db.Contact
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<ContactModel> GetByIdAsync(Guid id)
        {
            return await _db.Contact
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);
        }


    }
}
