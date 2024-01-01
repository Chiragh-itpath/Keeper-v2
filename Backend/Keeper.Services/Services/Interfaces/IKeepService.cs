using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IKeepService
    {
        Task<List<KeepViewModel>> GetAllAsync(Guid projectId, Guid userId);
        Task<KeepViewModel> GetAsync(Guid id);
        Task<KeepViewModel> AddAsync(AddKeep addKeep,Guid userId);
        Task<KeepViewModel> UpdateAsync(EditKeep editKeep, Guid userId);
        Task<bool> DeleteAsync(Guid id);
        Task<List<KeepUserViewModel>> AllInvitedUser(Guid keepId);
    }
}
