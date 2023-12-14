using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectViewModel>> GetAllAsync(Guid UserId);
        Task<ProjectViewModel> GetByIdAsync(Guid Id,Guid userId);
        Task<ProjectViewModel> SaveAsync(AddProject addProject,Guid userId);
        Task<ProjectViewModel> UpdateAsync(EditProject editProject, Guid userId);
        Task<bool> DeleteByIdAsync(Guid Id);
    }
}
