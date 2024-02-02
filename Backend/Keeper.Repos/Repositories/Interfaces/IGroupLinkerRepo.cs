using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IGroupLinkerRepo
    {
        Task<ContactGroupLinkerModel> AddAsync(ContactGroupLinkerModel item);
        Task<ContactGroupLinkerModel?> GetAsync(Guid contactId, Guid groupId);
        Task<List<ContactGroupLinkerModel>> GetListByContactId(Guid contactId);
        Task<int> RemoveAsync(ContactGroupLinkerModel linkerModel);
    }
}
