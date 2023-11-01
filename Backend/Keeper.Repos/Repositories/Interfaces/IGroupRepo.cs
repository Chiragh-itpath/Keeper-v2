using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IGroupRepo
    {
        public Task<GroupModel> AddAsync(GroupModel group);
        Task<GroupModel?> GetByIdAsync(Guid groupId);
        public Task<List<GroupModel>> GetAllAsync(Guid userId);
    }
}
