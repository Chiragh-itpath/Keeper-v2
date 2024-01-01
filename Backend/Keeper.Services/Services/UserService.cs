using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Interfaces;
using Keeper.Services.Interfaces;

namespace KeeperCore.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public UserViewModel MapToUserVM(UserModel user)
        {
            return new UserViewModel 
            { 
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Mobile = user.Mobile
            };
        }
        public async Task<UserViewModel> GetByIdAsync(Guid id)
        {
            var user = await _userRepo.GetById(id) ?? throw new InnerException("No User found with this id", StatusType.NOT_FOUND);
            return MapToUserVM(user);
        }
        public async Task<UserViewModel> CheckEmail(string email)
        {
            var res = await _userRepo.GetByEmailAsync(email);
            return res == null ? throw new InnerException("Email is not registered", StatusType.EMAIL_NOT_FOUND) : MapToUserVM(res);
        }
        public async Task<List<UserViewModel>> EmailSearch(string email, Guid userId)
        {
            var result = await _userRepo.GetEmailList(email, userId);
            var userList = result.Select(x => MapToUserVM(x)).ToList();
            return userList;
        }
    }
}
