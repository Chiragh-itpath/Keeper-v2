using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupViewModel> AddAsync(AddGroup addGroup, Guid userId);
        Task<List<GroupViewModel>> GetAllGroup(Guid userId);
        Task<GroupViewModel> AddContacts(AddContactsToGroup addContacts);
    }
}
