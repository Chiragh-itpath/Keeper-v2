using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class GroupLinkerRepo : IGroupLinkerRepo
    {
        private readonly DbKeeperContext _db;
        public GroupLinkerRepo(DbKeeperContext db)
        {
            _db = db;
        }

        public async Task<ContactGroupLinkerModel> AddAsync(ContactGroupLinkerModel item)
        {
            await _db.ContactGroupLinkers.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }
        public async Task<ContactGroupLinkerModel?> GetAsync(Guid contactId, Guid groupId)
        {
            return await _db.ContactGroupLinkers
                .FirstOrDefaultAsync(x => x.ContactId == contactId && x.GroupId == groupId);
        }
        public async Task<List<ContactGroupLinkerModel>> GetListByContactId(Guid contactId)
        {
            return await _db.ContactGroupLinkers
                .Where(x => x.ContactId == contactId)
                .ToListAsync();
        }
        public async Task<int> RemoveAsync(ContactGroupLinkerModel linkerModel)
        {
            _db.Remove(linkerModel);
            return await _db.SaveChangesAsync();
        }

    }
}
