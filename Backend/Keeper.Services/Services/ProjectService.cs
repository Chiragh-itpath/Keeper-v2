using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _projectRepo;
        private readonly ITagService _tagService;
        private readonly IProjectShareRepo _projectShareRepo;
        private readonly IUserService _userService;
        public ProjectService(IProjectRepo projectRepo, ITagService tagService, IProjectShareRepo projectShareRepo, IUserService userService)
        {
            _projectRepo = projectRepo;
            _tagService = tagService;
            _projectShareRepo = projectShareRepo;
            _userService = userService;
        }
        public async Task<List<ProjectViewModel>> GetAllAsync(Guid UserId)
        {
            List<ProjectModel> result = await _projectRepo.GetAllAsync(UserId);
            List<ProjectViewModel> projects = new();
            foreach (var item in result)
            {
                ProjectViewModel project = ProjectViewModelMapper(item, UserId);
                project.Users = await AllInvitedUsers(item.Id);
                projects.Add(project);
            }
            return projects;
        }
        public async Task<ProjectViewModel> GetByIdAsync(Guid Id, Guid userId)
        {
            ProjectModel project = await _projectRepo.GetByIdAsync(Id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            ProjectViewModel projectViewModel = ProjectViewModelMapper(project, userId);
            projectViewModel.Users = await AllInvitedUsers(project.Id);
            return projectViewModel;
        }
        public async Task<ProjectViewModel> SaveAsync(AddProject addProject, Guid userId)
        {
            ProjectModel project = new()
            {
                Title = addProject.Title,
                Description = addProject.Description,
                CreatedOn = DateTime.Now,
                CreatedById = userId
            };
            if (!string.IsNullOrEmpty(addProject.Tag))
            {
                TagModel? tag = await _tagService.AddAsync(addProject.Tag, userId, TagType.PROJECT);
                project.TagId = tag?.Id;
            }
            Guid ProjectId = await _projectRepo.SaveAsync(project);
            ProjectModel savedProject = await _projectRepo.GetByIdAsync(ProjectId) ?? throw new InnerException("", StatusType.NOT_FOUND);
            ProjectViewModel projectView = ProjectViewModelMapper(savedProject, userId);
            return projectView;
        }
        public async Task<ProjectViewModel> UpdateAsync(EditProject editProject, Guid userId)
        {
            ProjectModel project = await _projectRepo.GetByIdAsync(editProject.Id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            project.Title = editProject.Title;
            project.Description = editProject.Description;
            project.UpdatedById = userId;
            project.UpdatedOn = DateTime.Now;
            if (!string.IsNullOrEmpty(editProject.Tag))
            {
                TagModel? tag = await _tagService.AddAsync(editProject.Tag, project.CreatedById, TagType.PROJECT);
                project.TagId = tag?.Id;
            }
            Guid projectId = await _projectRepo.UpdateAsync(project);
            ProjectViewModel projectView = await GetByIdAsync(projectId, userId);
            return projectView;
        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            ProjectModel? project = await _projectRepo.GetByIdAsync(id);
            if (project != null)
            {
                await _projectRepo.DeleteAsync(project);
            }
            return true;
        }
        public static ProjectViewModel ProjectViewModelMapper(ProjectModel project, Guid userId)
        {
            return new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Tag = project.Tag?.Title,
                CreatedBy = project.CreatedBy.Email,
                UpdatedBy = project.UpdatedBy?.Email,
                CreatedOn = project.CreatedOn,
                UpdatedOn = project.UpdatedOn,
                IsShared = project.CreatedBy.Id != userId,
            };
        }
        public async Task<List<ProjectUsersViewModel>> AllInvitedUsers(Guid ProjectId)
        {
            List<SharedProjectsModel> userList = await _projectShareRepo.GetAllAsync(ProjectId);
            return userList
                .Select(shared => new ProjectUsersViewModel
                {
                    ShareId = shared.Id,
                    InvitedUser = _userService.MapToUserVM(shared.User),
                    IsAccepted = shared.IsAccepted
                })
                .ToList();
        }
    }
}
