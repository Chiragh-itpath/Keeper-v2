using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IStatusService
    {
        Task<StatusViewModel> AddStatusAsync(AddStauts addStauts, Guid userId);
        Task<StatusViewModel> UpdateStatusAsync(EditStatus editStatus, Guid userId);
        Task<bool> DeleteStatusAsync(Guid id);
        Task<List<StatusViewModel>> GetAll(Guid ProjectId);
    }
}
