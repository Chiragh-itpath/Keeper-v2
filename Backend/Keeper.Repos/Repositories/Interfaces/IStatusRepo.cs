using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IStatusRepo
    {
        Task<ItemStatusModel> AddStatusAsync(ItemStatusModel status);
        Task<ItemStatusModel> UpdateStatusAsync(ItemStatusModel status);
        Task<List<ItemStatusModel>> GetAllAsync(Guid ProjectId);
        Task<ItemStatusModel?> GetByIdAsync(Guid Id);
        Task<int> RemoveAsync(ItemStatusModel item);
    }
}