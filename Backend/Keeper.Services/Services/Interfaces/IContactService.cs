using Keeper.Common.Response;
using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IContactService
    {
        Task<ResponseModel<ContactViewModel>> AddAsync(AddContact contact, Guid userId);
        Task<ResponseModel<List<ContactViewModel>>> GetAllContacts(Guid userId);
        Task<ContactViewModel> GetById(Guid id);
    }
}
