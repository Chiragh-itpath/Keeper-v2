using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class KeepService : IKeepService
    {
        private readonly IKeepRepo _keepRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly IProjectShareRepo _projectShareRepo;
        private readonly IKeepShareRepo _keepShareRepo;
        private readonly ITagService _tag;
        private readonly IUserService _user;
        public KeepService(IKeepRepo keepRepo, ITagService tagService, IProjectRepo projectRepo, IProjectShareRepo projectShareRepo, IKeepShareRepo keepShareRepo, IUserService user)
        {
            _keepRepo = keepRepo;
            _tag = tagService;
            _projectRepo = projectRepo;
            _projectShareRepo = projectShareRepo;
            _keepShareRepo = keepShareRepo;
            _user = user;
        }
        public async Task<List<KeepViewModel>> GetAllAsync(Guid projectId, Guid userId)
        {
            List<KeepModel> result;
            var project = await _projectRepo.GetByIdAsync(projectId);
            var sharedProject = await _projectShareRepo.GetAsync(projectId, userId);
            if (project?.CreatedById == userId || sharedProject != null)
                result = await _keepRepo.GetAllAsync(projectId);
            else
                result = await _keepRepo.GetAllShared(projectId, userId);

            var keeps = result.Select(item => Mapper(item)).ToList();
            return keeps;
        }
        public async Task<KeepViewModel> GetAsync(Guid id)
        {
            var result = await _keepRepo.GetAsync(id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            return Mapper(result);
        }
        public async Task<KeepViewModel> AddAsync(AddKeep addKeep, Guid userId)
        {
            KeepModel keep = new()
            {
                Title = addKeep.Title,
                ProjectId = addKeep.ProjectId,
                CreatedById = userId,
                CreatedOn = DateTime.Now,
            };

            if (!string.IsNullOrEmpty(addKeep.Tag))
            {
                var project = await _projectRepo.GetByIdAsync(addKeep.ProjectId);
                var tag = await _tag.AddAsync(addKeep.Tag, project!.CreatedById, TagType.KEEP);
                keep.TagId = tag?.Id;
            }
            var keepId = await _keepRepo.SaveAsync(keep);
            var res = await GetAsync(keepId);
            return res;
        }
        public async Task<KeepViewModel> UpdateAsync(EditKeep editKeep, Guid userId)
        {
            var keep = await _keepRepo.GetAsync(editKeep.Id) ?? throw new InnerException("", StatusType.SUCCESS);
            keep.Title = editKeep.Title;
            keep.UpdatedOn = DateTime.Now;
            keep.UpdatedById = userId;
            if (!string.IsNullOrEmpty(editKeep.Tag))
            {
                var project = await _projectRepo.GetByIdAsync(editKeep.ProjectId);
                var tag = await _tag.AddAsync(editKeep.Tag, project!.CreatedById, TagType.KEEP);
                keep.TagId = tag?.Id;
            }
            var keepId = await _keepRepo.UpdateAsync(keep);
            var res = await GetAsync(keepId);
            return res;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var keep = await _keepRepo.GetAsync(id) ?? throw new InnerException("", StatusType.NOT_FOUND);
            await _keepRepo.DeleteAsync(keep);
            return true;
        }
        private static KeepViewModel Mapper(KeepModel keep)
        {
            return new KeepViewModel
            {
                Id = keep.Id,
                Title = keep.Title,
                ProjectId = keep.ProjectId,
                CreatedBy = keep.CreatedBy.Email,
                CreatedOn = keep.CreatedOn,
                Updatedby = keep.UpdatedBy?.Email,
                UpdatedOn = keep.UpdatedOn,
                Tag = keep.Tag?.Title,
            };
        }
        public async Task<List<KeepUserViewModel>> AllInvitedUser(Guid keepId)
        {
            await Task.CompletedTask;
            var userList = await _keepShareRepo.GetAllAsync(keepId);
            return userList
                .Select(share => new KeepUserViewModel
                {
                    ShareId = share.Id,
                    IsAccepted = share.IsAccepted,
                    InvitedUser = _user.MapToUserVM(share.User)
                })
                .ToList();
        }
    }
}
