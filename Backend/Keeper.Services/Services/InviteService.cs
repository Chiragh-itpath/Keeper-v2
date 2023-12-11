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
        private readonly IProjectShareRepo _projectShare;
        private readonly IKeepShareRepo _shareKeep;
        private readonly IUserRepo _user;
        private readonly IProjectRepo _project;
        private readonly IKeepRepo _keep;
        private readonly IMailService _mail;
        public InviteService(IProjectShareRepo projectShare, IUserRepo user, IProjectRepo project, IKeepShareRepo shareKeep, IKeepRepo keep, IMailService mail)
        {
            _projectShare = projectShare;
            _user = user;
            _project = project;
            _shareKeep = shareKeep;
            _keep = keep;
            _mail = mail;
        }
        public async Task<bool> InviteToProjectAsync(ProjectInviteModel invite, Guid userId)
        {
            var sender = await _user.GetById(userId);
            var project = await _project.GetByIdAsync(invite.ProjectId);
            int skipCount = 0;
            for (int i = 0; i < invite.Emails.Count; i++)
            {
                var user = await _user.GetByEmailAsync(invite.Emails[i]);
                var shared = await _projectShare.GetAsync(invite.ProjectId, user!.Id);
                if (shared != null)
                {
                    skipCount++;
                    continue;
                }
                var inviteModel = new SharedProjectsModel()
                {
                    ProjectId = invite.ProjectId,
                    UserId = user.Id
                };
                await _projectShare.AddAsync(inviteModel);
                await _mail.SendEmailAsync(new MailModel
                {
                    Category = MailCategory.SendInvitation,
                    From = sender?.Email ?? string.Empty,
                    To = user.Email,
                    Subject = "Project Invitation",
                    Message = project?.Title ?? string.Empty
                });
            }
            if (skipCount == invite.Emails.Count) throw new InnerException("All User already invited", StatusType.NOT_VALID);
            return true;
        }
        public async Task<List<InvitedProjectModel>> GetAllInvitedProject(Guid userId)
        {
            var invited = (await _projectShare.GetAllInvited(userId))
                .Where(x => !x.IsAccepted)
                .ToList();

            var tasks = invited.Select(async invitation =>
            {
                var project = await _project.GetByIdAsync(invitation.ProjectId);
                var user = await _user.GetById(project?.CreatedById ?? Guid.Empty);

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
            var shared = await _projectShare.GetAsync(projectInvite.InviteId);
            var project = await _project.GetByIdAsync(shared.ProjectId);
            var user = project?.CreatedBy;
            var sender = await _user.GetById(userId);
            if (projectInvite.Response)
            {
                shared.IsAccepted = true;
                await _projectShare.UpdateAsync(shared);
            }
            else
            {
                await _projectShare.DeleteAsync(shared);
            }
            await _mail.SendEmailAsync(new MailModel
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
            var sender = await _user.GetById(userId);
            var keep = await _keep.GetAsync(invite.KeepId);
            int skipcount = 0;
            for (int i = 0; i < invite.Emails.Count; i++)
            {
                var user = await _user.GetByEmailAsync(invite.Emails[i]);
                var shared = await _shareKeep.GetAsync(keep!.Id, user!.Id);
                if (shared != null)
                {
                    skipcount++;
                    continue;
                }
                var inviteModel = new SharedKeepsModel()
                {
                    KeepId = invite.KeepId,
                    ProjectId = invite.ProjectId,
                    UserId = user!.Id
                };
                await _shareKeep.AddAsync(inviteModel);
                await _mail.SendEmailAsync(new MailModel
                {
                    Category = MailCategory.SendInvitation,
                    From = sender?.Email ?? string.Empty,
                    To = user.Email,
                    Subject = "Project Invitation",
                    Message = keep?.Title ?? ""
                });
            }
            if (skipcount == invite.Emails.Count) throw new InnerException("All User already Invited", StatusType.NOT_VALID);
            return true;
        }
        public async Task<List<InviteKeepModel>> GetAllInvitedKeep(Guid UserId)
        {
            var invited = await _shareKeep.GetAllInvited(UserId);
            invited = invited.Where(x => !x.IsAccepted).ToList();
            List<InviteKeepModel> inviteKeeps = new();
            for (int i = 0; i < invited.Count; i++)
            {
                var keep = await _keep.GetAsync(invited[i].KeepId);
                var user = await _user.GetById(keep!.CreatedById);
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
            var shared = await _shareKeep.GetAsync(keepInvite.InviteId);
            var project = await _project.GetByIdAsync(shared.ProjectId);
            var user = project?.CreatedBy;
            var sender = await _user.GetById(userId);
            if (keepInvite.Response)
            {
                shared.IsAccepted = true;
                await _shareKeep.UpdateAsync(shared);
            }
            else
            {
                await _shareKeep.DeleteAsync(shared);
            }
            await _mail.SendEmailAsync(new MailModel
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
            var SharedProject = await _projectShare.GetAllAsync(projectId);
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
            var sharedkeep = await _shareKeep.GetAllAsync(keepId);
            var collaborator = sharedkeep.Select(x => new Collaborator
            {
                UserName = x.User.UserName,
                Email = x.User.Email,
                HasAccepted = x.IsAccepted
            }).ToList();
            return collaborator;
        }
    }
}
