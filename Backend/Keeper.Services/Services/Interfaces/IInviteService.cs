using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IInviteService
    {
        Task<bool> InviteToProjectAsync(ProjectInviteModel invite,Guid userId);
        Task<List<InvitedProjectModel>> GetAllInvitedProject(Guid UserId);
        Task<bool> ResponseToProjectInvite(InviteResponseModel projectInvite,Guid userId);
        Task<bool> InviteToKeepAsync(KeepInviteModel invite,Guid userId);
        Task<List<InviteKeepModel>> GetAllInvitedKeep(Guid UserId);
        Task<bool> ResponseToKeepInvite(InviteResponseModel keepInvite,Guid userId);
        Task<List<Collaborator>> GetProjectCollaborators(Guid projectId);
        Task<List<Collaborator>> GetKeepCollaborators(Guid keepId);
        Task<int> RemoveFromProject(Guid ShareId);
        Task<int> RemoveFromKeep(Guid shareId);
    }
}
