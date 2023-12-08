using Keeper.Common.ViewModels;

namespace Keeper.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetByIdAsync(Guid id);
        Task<UserViewModel> CheckEmail(string email);
        Task<List<string>> EmailSearch(string email, Guid userId);
    }
}
