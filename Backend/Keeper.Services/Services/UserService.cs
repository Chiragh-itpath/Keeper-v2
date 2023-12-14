using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.Response;
using Keeper.Common.ViewModels;
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
        public async Task<UserViewModel> GetByIdAsync(Guid id)
        {

            var user = await _userRepo.GetById(id) ?? throw new InnerException("No User found with this id", StatusType.NOT_FOUND);
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Contact = user.Contact
            };
        }
        public async Task<UserViewModel> CheckEmail(string email)
        {
            var res = await _userRepo.GetByEmailAsync(email);
            if (res == null) throw new InnerException("Email is not registered", StatusType.EMAIL_NOT_FOUND);
            return new UserViewModel
            {
                Id = res.Id,
                UserName = res.UserName,
                Email = res.Email,
                Contact = res.Contact
            };
        }
        public async Task<List<string>> EmailSearch(string email, Guid userId)
        {
            var result = await _userRepo.GetEmailList(email, userId);
            var userList = result.Select(x => x.Email).ToList();
            return userList;
        }
    }
}
