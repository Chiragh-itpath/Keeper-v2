using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactViewModel>> AddAsync(AddContact contact, Guid userId);
        Task<List<ContactViewModel>> GetAllContacts(Guid userId);
        Task<ContactViewModel> GetById(Guid id);
    }
}
