using Keeper.Repos.Repositories.Interfaces;
using Keeper.Context.Model;
using Keeper.Context;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class StatusRepo : IStatusRepo
    {
        private readonly DbKeeperContext _context;
        public StatusRepo(DbKeeperContext context)
        {
            _context = context;
        }

        public async Task<ItemStatusModel> AddStatusAsync(ItemStatusModel status)
        {
            await _context.AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<ItemStatusModel> UpdateStatusAsync(ItemStatusModel status)
        {
            _context.Entry(status).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<List<ItemStatusModel>> GetAllAsync(Guid ProjectId)
        {
            return await _context.ItemStatus
                .AsNoTracking()
                .Where(x => x.ProjectId == ProjectId)
                .ToListAsync();
        }

        public async Task<ItemStatusModel?> GetByIdAsync(Guid Id)
        {
            return await _context.ItemStatus
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> RemoveAsync(ItemStatusModel item)
        {
            _context.ItemStatus.Remove(item);
            return await _context.SaveChangesAsync();
        }
    }
}