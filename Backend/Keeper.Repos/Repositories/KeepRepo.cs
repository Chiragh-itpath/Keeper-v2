using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class KeepRepo : IKeepRepo
    {
        private readonly DbKeeperContext _db;
        public KeepRepo(DbKeeperContext dbKeeperContext)
        {
            _db = dbKeeperContext;
        }

        public async Task<List<KeepModel>> GetAllAsync(Guid ProjectId)
        {
            return await _db.Keeps
                .Include(k => k.Tag)
                .Include(k => k.CreatedBy)
                .Include(k => k.UpdatedBy)
                .AsNoTracking()
                .Where(x => x.ProjectId == ProjectId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.UpdatedOn)
                .ToListAsync();
        }

        public async Task<List<KeepModel>> GetAllShared(Guid projectId, Guid UserId)
        {
            List<KeepModel> keeps = await (
                from k in _db.Keeps
                .Include(k => k.Tag)
                .Include(k => k.CreatedBy)
                .Include(k => k.UpdatedBy)
                join sk in _db.SharedKeeps on
                  new { KeepId = k.Id, k.ProjectId, UserId } equals
                  new { sk.KeepId, sk.ProjectId, sk.UserId }
                where !k.IsDeleted && k.ProjectId == projectId
                select k)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .ThenByDescending(x => x.UpdatedOn)
                .ToListAsync();
            return keeps;
        }

        public async Task<KeepModel?> GetAsync(Guid id)
        {
            return await _db.Keeps
                .Include(k => k.Tag)
                .Include(k => k.CreatedBy)
                .Include(k => k.UpdatedBy)
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> SaveAsync(KeepModel keep)
        {
            await _db.Keeps.AddAsync(keep);
            await _db.SaveChangesAsync();
            return keep.Id;
        }

        public async Task<Guid> UpdateAsync(KeepModel Keep)
        {
            _db.Entry(Keep).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Keep.Id;
        }

        public async Task DeleteAsync(KeepModel keep)
        {
            keep.IsDeleted = true;
            await UpdateAsync(keep);
        }
    }
}
