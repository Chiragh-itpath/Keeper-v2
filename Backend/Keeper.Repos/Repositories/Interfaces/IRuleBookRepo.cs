using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IRuleBookRepo
    {
        Task<RuleBookModel> AddAsync(RuleBookModel ruleBook);
        Task<RuleBookModel> UpdateAsync(RuleBookModel ruleBook);
        Task<RuleBookModel?> GetByProjectId(Guid projectId);
    }
}
