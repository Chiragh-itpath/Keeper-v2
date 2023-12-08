using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _repo;
        private readonly ITagService _tag;
        public ProjectService(IProjectRepo repo, ITagService tagService)
        {
            _repo = repo;
            _tag = tagService;

        }
        public async Task<List<ProjectViewModel>> GetAllAsync(Guid UserId)
        {
            var result = await _repo.GetAllAsync(UserId);

            var projects = result
                .Select(project => Mapper(project))
                .ToList();
            return projects;
        }
        public async Task<ProjectViewModel> GetByIdAsync(Guid Id)
        {
            var result = await _repo.GetByIdAsync(Id) ?? throw new InnerException("",StatusType.NOT_FOUND);
            return Mapper(result);
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
                var tag = await _tag.AddAsync(addProject.Tag, userId, TagType.PROJECT);
                project.TagId = tag?.Id;
            }
            var projectId = await _repo.SaveAsync(project);
            var res = await GetByIdAsync(projectId);
            return res;
        }
        public async Task<ProjectViewModel> UpdateAsync(EditProject editProject, Guid userId)
        {
            var project = await _repo.GetByIdAsync(editProject.Id) ?? throw new InnerException("",StatusType.NOT_FOUND);
            project.Title = editProject.Title;
            project.Description = editProject.Description;
            project.UpdatedById = userId;
            project.UpdatedOn = DateTime.Now;
            if (!string.IsNullOrEmpty(editProject.Tag))
            {
                var tag = await _tag.AddAsync(editProject.Tag, project.CreatedById, TagType.PROJECT);
                project.TagId = tag?.Id;
            }
            else
            {
                project.TagId = null;
            }
            var projectId = await _repo.UpdateAsync(project);
            var res = await GetByIdAsync(projectId);
            return res;
        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var project = await _repo.GetByIdAsync(id);
            await _repo.DeleteAsync(project!);
            return true;
        }
        private static ProjectViewModel Mapper(ProjectModel project)
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
            };
        }
    }
}
