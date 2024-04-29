using Keeper.Context;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Repos.Repositories
{
    public class RuleBookRepo : IRuleBookRepo
    {
        private readonly DbKeeperContext _db;

        public RuleBookRepo(DbKeeperContext db)
        {
            _db = db;
        }

        public async Task<RuleBookModel> AddAsync(RuleBookModel ruleBook)
        {
            await _db.RuleBook.AddAsync(ruleBook);
            await _db.SaveChangesAsync();
            return ruleBook;
        }

        public async Task<RuleBookModel> UpdateAsync(RuleBookModel ruleBook)
        {
            _db.Entry(ruleBook).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ruleBook;
        }

        public async Task<RuleBookModel?> GetByProjectId(Guid projectId)
        {
            return await _db.RuleBook
                .Where(x => x.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

    }
}
