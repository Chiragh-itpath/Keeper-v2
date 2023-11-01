using Keeper.Common.Response;
using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ResponseModel<GroupViewModel>> AddAsync(AddGroup addGroup, Guid userId);
        Task<ResponseModel<List<GroupViewModel>>> GetAllGroup(Guid userId);
        Task<ResponseModel<GroupViewModel>> AddContacts(AddContactsToGroup addContacts);
    }
}
