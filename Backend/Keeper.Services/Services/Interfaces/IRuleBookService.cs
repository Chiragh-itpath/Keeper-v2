using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IRuleBookService
    {
        Task<RuleBookViewModel> AddOrUpdateAsync(AddRule rule, Guid userId);
        Task<RuleBookViewModel> GetByProjectId(Guid projectId);
    }
}
