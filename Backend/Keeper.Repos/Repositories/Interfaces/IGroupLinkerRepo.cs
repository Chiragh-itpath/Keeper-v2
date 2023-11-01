using Keeper.Context.Model;

namespace Keeper.Repos.Repositories.Interfaces
{
    public interface IGroupLinkerRepo
    {
        Task<ContactGroupLinkerModel> AddAsync(ContactGroupLinkerModel item);
    }
}
