using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IClientRepo
    {
        Task<ClientModel> AddAsync(ClientModel client);
        Task<ClientModel> UpdateAsync(ClientModel client);
        Task<int> RemoveAsync(ClientModel client);
        Task<List<ClientModel>> GetAllAsync(Guid ProjectId);
        Task<ClientModel?> GetByIdAsync(Guid Id);
    }
}
