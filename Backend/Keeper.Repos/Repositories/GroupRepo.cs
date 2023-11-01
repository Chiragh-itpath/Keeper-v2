using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class GroupRepo : IGroupRepo
    {
        private readonly DbKeeperContext _db;
        public GroupRepo(DbKeeperContext db)
        {
            _db = db;
        }
        public async Task<GroupModel> AddAsync(GroupModel group)
        {
            await _db.AddAsync(group);
            await _db.SaveChangesAsync();
            return group;
        }
        public async Task<GroupModel?> GetByIdAsync(Guid groupId)
        {
            return await _db.Group
                .Include(x => x.User)
                .Include(x => x.Linkers)
                .Where(x => x.Id == groupId).FirstOrDefaultAsync();
        }
        public async Task<List<GroupModel>> GetAllAsync(Guid userId)
        {
            return await _db.Group
                .Include(x => x.User)
                .Include(x => x.Linkers)
                .Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
