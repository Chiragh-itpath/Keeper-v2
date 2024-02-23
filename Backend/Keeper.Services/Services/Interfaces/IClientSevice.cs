using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IClientSevice
    {
        Task<List<ClientViewModel>> GetAllAsync(Guid ProjectId);
        Task<ClientViewModel> AddAsync(AddClient addClient, Guid userId);
        Task<ClientViewModel> UpdateAsync(EditClient editClient, Guid userId);
        Task<bool> DeleteAsync(Guid Id);
    }
}
