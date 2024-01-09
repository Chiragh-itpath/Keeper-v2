using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.OtherModels;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Interfaces;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class InviteService : IInviteService
    {
        private readonly IProjectShareRepo _projectShareRepo;
        private readonly IKeepShareRepo _keepShareRepo;
        private readonly IUserRepo _userRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly IKeepRepo _keepRepo;
        private readonly IMailService _mailService;
        public InviteService(IProjectShareRepo projectShare, IUserRepo user, IProjectRepo project, IKeepShareRepo shareKeep, IKeepRepo keep, IMailService mail)
        {
            _projectShareRepo = projectShare;
            _userRepo = user;
            _projectRepo = project;
            _keepShareRepo = shareKeep;
            _keepRepo = keep;
            _mailService = mail;
        }
        public async Task<bool> InviteToProjectAsync(ProjectInviteModel invite, Guid userId)
        {
            var sender = await _userRepo.GetById(userId);
            var project = await _projectRepo.GetByIdAsync(invite.ProjectId);
            for (int i = 0; i < invite.Users.Count; i++)
            {
                var shared = await _projectShareRepo.GetAsync(invite.ProjectId, invite.Users[i].Id);
                if (shared != null) continue;
                var inviteModel = new SharedProjectsModel()
                {
                    ProjectId = invite.ProjectId,
                    UserId = invite.Users[i].Id,
                    Permission = invite.Users[i].Permission
                };
                await _projectShareRepo.AddAsync(inviteModel);
                await _mailService.SendEmailAsync(new MailModel
                {
                    Category = MailCategory.SendInvitation,
                    From = sender?.Email ?? string.Empty,
                    To = invite.Users[i].Email,
                    Subject = "Project Invitation",
                    Message = project?.Title ?? string.Empty
                });
            }
            return true;
        }
        public async Task<List<InvitedProjectModel>> GetAllInvitedProject(Guid userId)
        {
            var invited = (await _projectShareRepo.GetAllInvited(userId))
                .Where(x => !x.IsAccepted)
                .ToList();

            var tasks = invited.Select(async invitation =>
            {
                var project = await _projectRepo.GetByIdAsync(invitation.ProjectId);
                var user = await _userRepo.GetById(project?.CreatedById ?? Guid.Empty);

                if (project != null && user != null)
                {
                    return new InvitedProjectModel
                    {
                        ProjectId = invitation.Id,
                        Name = project.Title,
                        Email = user.Email
                    };
                }
                return null;
            }).ToList();
            var results = await Task.WhenAll(tasks);
            var invitedProjects = results.Where(result => result != null).ToList();
            return invitedProjects!;
        }

        public async Task<bool> ResponseToProjectInvite(InviteResponseModel projectInvite, Guid userId)
        {
            var shared = await _projectShareRepo.GetAsync(projectInvite.InviteId);
            var project = await _projectRepo.GetByIdAsync(shared.ProjectId);
            var user = project?.CreatedBy;
            var sender = await _userRepo.GetById(userId);
            if (projectInvite.Response)
            {
                shared.IsAccepted = true;
                await _projectShareRepo.UpdateAsync(shared);
            }
            else
            {
                await _projectShareRepo.DeleteAsync(shared);
            }
            await _mailService.SendEmailAsync(new MailModel
            {
                Category = projectInvite.Response ? MailCategory.AcceptInvitation : MailCategory.RejectInvitation,
                From = sender?.Email ?? "",
                To = user?.Email ?? "",
                Subject = $"Invitation {(projectInvite.Response ? "Accepted" : "Rejected")}"
            });
            return projectInvite.Response;
        }
        public async Task<bool> InviteToKeepAsync(KeepInviteModel invite, Guid userId)
        {
            var sender = await _userRepo.GetById(userId);
            var keep = await _keepRepo.GetAsync(invite.KeepId);
            for (int i = 0; i < invite.Users.Count; i++)
            {
                var projectShared = (await _projectShareRepo.GetAsync(invite.ProjectId, invite.Users[i].Id)) != null;
                if (projectShared) continue;
                var keepShared = (await _keepShareRepo.GetAsync(keep!.Id, invite.Users[i].Id)) != null;
                if (keepShared) continue;
                var inviteModel = new SharedKeepsModel()
                {
                    KeepId = invite.KeepId,
                    ProjectId = invite.ProjectId,
                    Permission = invite.Users[i].Permission,
                    UserId = invite.Users[i].Id
                };
                await _keepShareRepo.AddAsync(inviteModel);
                await _mailService.SendEmailAsync(new MailModel
                {
                    Category = MailCategory.SendInvitation,
                    From = sender?.Email ?? string.Empty,
                    To = invite.Users[i].Email,
                    Subject = "Project Invitation",
                    Message = keep?.Title ?? ""
                });
            }
            return true;
        }
        public async Task<List<InviteKeepModel>> GetAllInvitedKeep(Guid UserId)
        {
            var invited = await _keepShareRepo.GetAllInvited(UserId);
            invited = invited.Where(x => !x.IsAccepted).ToList();
            List<InviteKeepModel> inviteKeeps = new();
            for (int i = 0; i < invited.Count; i++)
            {
                var keep = await _keepRepo.GetAsync(invited[i].KeepId);
                var user = await _userRepo.GetById(keep!.CreatedById);
                inviteKeeps.Add(new InviteKeepModel
                {
                    KeepId = invited[i].Id,
                    Name = keep.Title,
                    Email = user!.Email
                });
            }
            return inviteKeeps;
        }
        public async Task<bool> ResponseToKeepInvite(InviteResponseModel keepInvite, Guid userId)
        {
            var shared = await _keepShareRepo.GetAsync(keepInvite.InviteId);
            var project = await _projectRepo.GetByIdAsync(shared.ProjectId);
            var user = project?.CreatedBy;
            var sender = await _userRepo.GetById(userId);
            if (keepInvite.Response)
            {
                shared.IsAccepted = true;
                await _keepShareRepo.UpdateAsync(shared);
            }
            else
            {
                await _keepShareRepo.DeleteAsync(shared);
            }
            await _mailService.SendEmailAsync(new MailModel
            {
                Category = keepInvite.Response ? MailCategory.AcceptInvitation : MailCategory.RejectInvitation,
                From = sender?.Email ?? "",
                To = user?.Email ?? "",
                Subject = $"Invitation {(keepInvite.Response ? "Accepted" : "Rejected")}"
            });
            return keepInvite.Response;
        }
        public async Task<List<Collaborator>> GetProjectCollaborators(Guid projectId)
        {
            var SharedProject = await _projectShareRepo.GetAllAsync(projectId);
            var collaborator = SharedProject.Select(x => new Collaborator
            {
                UserName = x.User.UserName,
                Email = x.User.Email,
                HasAccepted = x.IsAccepted
            }).ToList();

            return collaborator;
        }
        public async Task<List<Collaborator>> GetKeepCollaborators(Guid keepId)
        {
            var sharedkeep = await _keepShareRepo.GetAllAsync(keepId);
            var collaborator = sharedkeep.Select(x => new Collaborator
            {
                UserName = x.User.UserName,
                Email = x.User.Email,
                HasAccepted = x.IsAccepted
            }).ToList();
            return collaborator;
        }

        public async Task<int> RemoveFromProject(Guid ShareId)
        {
            var item = await _projectShareRepo.GetAsync(ShareId);
            return await _projectShareRepo.DeleteAsync(item);
        }

        public async Task<int> RemoveFromKeep(Guid shareId)
        {
            var item = await _keepShareRepo.GetAsync(shareId);
            return await _keepShareRepo.DeleteAsync(item);
        }

        public async Task UpdatePermissionOnProject(List<UpdatePermission> updatePermissionModel)
        {
            foreach (var permissionModel in updatePermissionModel)
            {
                SharedProjectsModel shareProjectModel = await _projectShareRepo.GetAsync(permissionModel.ShareId);
                shareProjectModel.Permission = permissionModel.Permission;
                await _projectShareRepo.UpdateAsync(shareProjectModel);
            }
        }
        public async Task UpdatePermissionOnKeep(List<UpdatePermission> updatePermissionModel)
        {
            foreach (var permissionModel in updatePermissionModel)
            {
                SharedKeepsModel sharedKeepsModel = await _keepShareRepo.GetAsync(permissionModel.ShareId);
                sharedKeepsModel.Permission = permissionModel.Permission;
                await _keepShareRepo.UpdateAsync(sharedKeepsModel);
            }
        }
    }
}
