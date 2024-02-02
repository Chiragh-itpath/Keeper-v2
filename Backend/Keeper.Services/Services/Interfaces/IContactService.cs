using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactViewModel> AddAsync(AddContact contact, Guid userId);
        Task<ContactViewModel> UpdateAsync(ContactViewModel contact);
        Task DeleteAsync(Guid id);
        Task<List<ContactViewModel>> GetAllContacts(Guid userId);
        Task<ContactViewModel> GetById(Guid id);
    }
}
