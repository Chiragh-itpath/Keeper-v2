using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class RuleBookService : IRuleBookService
    {
        private readonly IRuleBookRepo _ruleBookRepo;

        public RuleBookService(IRuleBookRepo ruleBookRepo)
        {
            _ruleBookRepo = ruleBookRepo;
        }

        public async Task<RuleBookViewModel> AddOrUpdateAsync(AddRule rule, Guid userId)
        {
            var oldRule = await _ruleBookRepo.GetByProjectId(rule.projectId);
            if (oldRule == null)
            {
                return MapToViewModel(await _ruleBookRepo.AddAsync(new RuleBookModel
                {
                    Text = rule.Text,
                    ProjectId = rule.projectId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                }));
            }
            oldRule.Text = rule.Text;
            oldRule.UpdatedOn = DateTime.Now;
            oldRule.UpdatedBy = userId;
            return MapToViewModel(await _ruleBookRepo.UpdateAsync(oldRule));
        }
        private static RuleBookViewModel MapToViewModel(RuleBookModel rule)
        {
            return new()
            {
                ProjectId = rule.ProjectId,
                Text = rule.Text
            };
        }

        public async Task<RuleBookViewModel> GetByProjectId(Guid projectId)
        {
            var rule = await _ruleBookRepo.GetByProjectId(projectId) ?? throw new InnerException("", StatusType.NOT_FOUND);
            return MapToViewModel(rule);
        }
    }
}
