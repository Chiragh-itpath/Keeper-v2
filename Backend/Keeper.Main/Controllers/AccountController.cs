using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Keeper.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("Register")]
        public async Task<ResponseModel<string>> Register(RegisterModel register)
        {
            await _accountService.RegisterAsync(register);
            return new ResponseModel<string>
            {
                Message = "Registered successfully"
            };
        }
        [HttpPost("Login")]
        public async Task<ResponseModel<TokenModel>> Login(LoginModel loginVM)
        {
            var token = await _accountService.LoginAsync(loginVM);
            return new ResponseModel<TokenModel> { Data = token };
        }
        [HttpGet("otp")]
        public async Task<ResponseModel<string>> GetOTP(string email)
        {
            var otp = await _accountService.GetOTP(email);
            return new ResponseModel<string> { Data = otp };
        }
        [HttpPut("ResetPassword")]
        public async Task<ResponseModel<string>> ResetPassword(PasswordResetModel resetModel)
        {
            await _accountService.UpdatePasswordAsync(resetModel);
            return new ResponseModel<string>
            {
                Message = "Password changed successfully!"
            };
        }
    }
}
