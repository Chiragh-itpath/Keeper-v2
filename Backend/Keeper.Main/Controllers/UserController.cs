using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keeper.Main.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;

        public UserController(IUserService userService)
        {
            _user = userService;
        }

        [HttpGet("Me")]
        public async Task<ResponseModel<UserViewModel>> Me()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _user.GetByIdAsync(userId);
            return new ResponseModel<UserViewModel> { Data = res };
        }
        [HttpGet("EmailSearch")]
        public async Task<ResponseModel<List<UserViewModel>>> EmailSearch([FromQuery] string email)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _user.EmailSearch(email, userId);
            return new ResponseModel<List<UserViewModel>>
            {
                Data = res
            };
        }
        [AllowAnonymous]
        [HttpGet("CheckMail")]
        public async Task<ResponseModel<UserViewModel>> CheckEmail(string email)
        {
            var res = await _user.CheckEmail(email);
            return new ResponseModel<UserViewModel> { Data = res };
        }
    }
}
