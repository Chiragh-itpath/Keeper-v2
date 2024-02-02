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
            var user = await _userRepo.GetByEmailAsync(invite.Email) ?? throw new InnerException("No user Found with this email", StatusType.EMAIL_NOT_FOUND);
            var sender = await _userRepo.GetById(userId);
            var project = await _projectRepo.GetByIdAsync(invite.ProjectId);
            var shared = await _projectShareRepo.GetAsync(invite.ProjectId, user.Id);
            if (shared == null)
            {
                var inviteModel = new SharedProjectsModel()
                {
                    ProjectId = invite.ProjectId,
                    UserId = user.Id,
                    Permission = invite.Permission
                };
                await _projectShareRepo.AddAsync(inviteModel);
                await _mailService.SendEmailAsync(new MailModel
                {
                    Category = MailCategory.SendInvitation,
                    From = sender?.Email ?? string.Empty,
                    To = user.Email,
                    Subject = "Project Invitation",
                    Message = project?.Title ?? string.Empty
                });
            }
            return true;
        }
        public async Task<List<InvitedProjectModel>> GetAllInvitedProject(Guid userId)
        {
            var allInvited = (await _projectShareRepo.GetAllInvited(userId))
                .Where(x => !x.IsAccepted)
                .ToList();
            List<InvitedProjectModel> invitedProjects = new();
            foreach (var invited in allInvited)
            {
                var project = await _projectRepo.GetByIdAsync(invited.ProjectId);
                if (project != null)
                {
                    invitedProjects.Add(new InvitedProjectModel
                    {
                        InviteId = invited.Id,
                        ProjectId = invited.ProjectId,
                        Name = project.Title,
                        Email = project.CreatedBy.Email
                    });
                }
            }
            return invitedProjects;
        }

        public async Task<bool> ResponseToProjectInvite(InviteResponseModel projectInvite, Guid userId)
        {
            var shared = await _projectShareRepo.GetByIdAsync(projectInvite.InviteId) ?? throw new InnerException("Invitation expired", StatusType.NOT_FOUND);
            var project = await _projectRepo.GetByIdAsync(shared.ProjectId) ?? throw new InnerException("Invited project no longer exist", StatusType.NOT_FOUND);
            var user = project.CreatedBy;
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
            var user = await _userRepo.GetByEmailAsync(invite.Email) ?? throw new InnerException("No User found with this email", StatusType.EMAIL_NOT_FOUND);
            var sender = await _userRepo.GetById(userId);
            var keep = await _keepRepo.GetAsync(invite.KeepId);

            var projectShared = (await _projectShareRepo.GetAsync(invite.ProjectId, user.Id)) != null;
            if (projectShared) return false;

            var keepShared = (await _keepShareRepo.GetAsync(keep!.Id, user.Id)) != null;
            if (keepShared) return false;

            var inviteModel = new SharedKeepsModel()
            {
                KeepId = invite.KeepId,
                ProjectId = invite.ProjectId,
                Permission = invite.Permission,
                UserId = user.Id
            };
            await _keepShareRepo.AddAsync(inviteModel);
            await _mailService.SendEmailAsync(new MailModel
            {
                Category = MailCategory.SendInvitation,
                From = sender?.Email ?? string.Empty,
                To = user.Email,
                Subject = "Project Invitation",
                Message = keep?.Title ?? ""
            });
            return true;
        }
        public async Task<List<InviteKeepModel>> GetAllInvitedKeep(Guid UserId)
        {
            var invited = await _keepShareRepo.GetAllInvited(UserId);
            invited = invited.Where(x => !x.IsAccepted).ToList();
            List<InviteKeepModel> inviteKeeps = new();
            foreach (var invite in invited)
            {
                var keep = await _keepRepo.GetAsync(invite.KeepId);
                var user = await _userRepo.GetById(keep!.CreatedById);
                inviteKeeps.Add(new InviteKeepModel
                {
                    InviteId = invite.Id,
                    ProjectId = invite.ProjectId,
                    KeepId = invite.KeepId,
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
            var item = await _projectShareRepo.GetByIdAsync(ShareId) ?? throw new InnerException("Not valid", StatusType.NOT_FOUND);
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
                var shareProjectModel = await _projectShareRepo.GetByIdAsync(permissionModel.ShareId)
                    ?? throw new InnerException("Not valid", StatusType.NOT_FOUND);
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
