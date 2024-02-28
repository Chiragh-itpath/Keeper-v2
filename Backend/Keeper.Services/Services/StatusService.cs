using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepo _StatusRepo;
        public StatusService(IStatusRepo statusRepo)
        {
            _StatusRepo = statusRepo;
        }
        private static StatusViewModel StatusMapper(ItemStatusModel status)
        {
            return new StatusViewModel
            {
                Id = status.Id,
                ProjectId = status.ProjectId,
                Title = status.Title,
                IsSystem = status.ProjectId == Guid.Empty
            };
        }
        public async Task<StatusViewModel> AddStatusAsync(AddStauts addStauts, Guid userId)
        {
            var status = await _StatusRepo.AddStatusAsync(new ItemStatusModel
            {
                Title = addStauts.Title,
                ProjectId = addStauts.ProjectId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            });
            return StatusMapper(status);
        }
        public async Task<StatusViewModel> UpdateStatusAsync(EditStatus editStatus, Guid userId)
        {
            var status = await _StatusRepo.GetByIdAsync(editStatus.Id) ?? throw new InnerException("Status not found", StatusType.NOT_FOUND);
            status.Title = editStatus.Title;
            status.UpdatedOn = DateTime.Now;
            status.UpdatedBy = userId;
            return StatusMapper(await _StatusRepo.UpdateStatusAsync(status));
        }
        public async Task<bool> DeleteStatusAsync(Guid id)
        {
            var status = await _StatusRepo.GetByIdAsync(id) ?? throw new InnerException("Status not found", StatusType.NOT_FOUND);
            if (status.ProjectId == Guid.Empty) throw new InnerException("Cannot delete system generated status", StatusType.UNAUTHORISED);
            if (await _StatusRepo.RemoveAsync(status) > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<List<StatusViewModel>> GetAll(Guid ProjectId, bool IncludeSystem = false)
        {
            var status = await _StatusRepo.GetAllAsync(ProjectId);
            if (IncludeSystem)
            {
                var defaultStatus = await _StatusRepo.GetAllAsync(Guid.Empty);
                status.AddRange(defaultStatus);
            }

            return status.Select(x => StatusMapper(x)).ToList();
        }
    }
}
