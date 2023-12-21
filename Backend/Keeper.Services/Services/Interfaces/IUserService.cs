using Keeper.Common.ViewModels;
using Keeper.Context.Model;

namespace Keeper.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetByIdAsync(Guid id);
        Task<UserViewModel> CheckEmail(string email);
        Task<List<UserViewModel>> EmailSearch(string email, Guid userId);
        UserViewModel MapToUserVM(UserModel user);
    }
}
