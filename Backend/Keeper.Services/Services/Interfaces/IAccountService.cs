using Keeper.Common.ViewModels;

namespace Keeper.Services.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterModel register);
        Task<TokenModel> LoginAsync(LoginModel loginVM);
        Task<string> GetOTP(string email);
        Task<bool> UpdatePasswordAsync(PasswordResetModel resetModel);
    }
}
