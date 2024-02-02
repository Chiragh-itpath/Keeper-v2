using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IContactRepo
    {
        Task<ContactModel> AddAsync(ContactModel contact);
        Task<ContactModel> UpdateAsync(ContactModel contact);
        Task DeleteAsync(ContactModel contact);
        Task<List<ContactModel>> GetAllAsync(Guid userId);
        Task<ContactModel> GetByIdAsync(Guid id);
    }
}
