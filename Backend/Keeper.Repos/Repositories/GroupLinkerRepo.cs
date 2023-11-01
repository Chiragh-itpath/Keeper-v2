using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;

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
    }
}
