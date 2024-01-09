using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Keeper.Main.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KeepController : ControllerBase
    {
        private readonly IKeepService _keepService;
        private readonly IInviteService _inviteService;
        public KeepController(IKeepService keepService, IInviteService inviteService)
        {
            _keepService = keepService;
            _inviteService = inviteService;
        }

        [HttpPost("")]
        public async Task<ResponseModel<KeepViewModel>> Post(AddKeep keep)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            KeepViewModel res = await _keepService.AddAsync(keep, userId);
            return new() { Data = res };
        }
        [HttpGet("")]
        public async Task<ResponseModel<List<KeepViewModel>>> GetAll(Guid ProjectId)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            List<KeepViewModel> res = await _keepService.GetAllAsync(ProjectId, userId);
            return new() { Data = res };
        }
        [HttpGet("{id}")]
        public async Task<ResponseModel<KeepViewModel>> GetById([FromRoute] Guid Id)
        {
            KeepViewModel res = await _keepService.GetAsync(Id);
            return new() { Data = res };
        }
        [HttpPut]
        public async Task<ResponseModel<KeepViewModel>> Update(EditKeep keep)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            KeepViewModel res = await _keepService.UpdateAsync(keep, userId);
            return new() { Data = res };
        }
        [HttpDelete("{Id}")]
        public async Task<ResponseModel<string>> Delete(Guid Id)
        {
            bool res = await _keepService.DeleteAsync(Id);
            return new() { Data = res.ToString() };
        }
        [HttpGet("Users/{KeepId}")]
        public async Task<ResponseModel<List<KeepUserViewModel>>> InvitedUser([FromRoute] Guid KeepId)
        {
            List<KeepUserViewModel> users = await _keepService.AllInvitedUser(KeepId);
            return new() { Data = users };
        }
        [HttpDelete("Remove/{shareId}")]
        public async Task<ResponseModel<string>> Remove([FromRoute] Guid shareId)
        {
            await Task.CompletedTask;
            var res = await _inviteService.RemoveFromKeep(shareId);
            return new() { Data = res.ToString() };
        }
        [HttpPut("UpdatePermission")]
        public async Task<ResponseModel<string>> UpdatePermission(List<UpdatePermission> updateModel)
        {
            await _inviteService.UpdatePermissionOnKeep(updateModel);
            return new()
            {
                Message = "Permission updated",
                Data = "success"
            };
        }
    }
}
