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
    public class InviteController : ControllerBase
    {
        private readonly IInviteService _invite;

        public InviteController(IInviteService invite)
        {
            _invite = invite;
        }

        [HttpPost("InviteToProject")]
        public async Task<ResponseModel<string>> InviteToProject(ProjectInviteModel invite)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            await _invite.InviteToProjectAsync(invite, userId);
            return new()
            {
                Message = "Invited"
            };
        }
        [HttpGet("AllInvitedProjects")]
        public async Task<ResponseModel<List<InvitedProjectModel>>> InvitedProjects()
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            List<InvitedProjectModel> invitedProjects = await _invite.GetAllInvitedProject(userId);
            return new()
            {
                Data = invitedProjects
            };
        }
        [HttpPost("ProjectInviteResponse")]
        public async Task<ResponseModel<string>> ProjectInviteResponse(InviteResponseModel response)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            bool res = await _invite.ResponseToProjectInvite(response, userId);
            return new()
            {
                Message = $"Inviation {(res ? "Accepted" : "Declied")}"
            };
        }
        [HttpPost("InviteToKeep")]
        public async Task<ResponseModel<string>> InviteToKeep(KeepInviteModel invite)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            await _invite.InviteToKeepAsync(invite, userId);
            return new()
            {
                Message = "Invited"
            };
        }
        [HttpGet("InvitedKeeps")]
        public async Task<ResponseModel<List<InviteKeepModel>>> InvitedKeeps()
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            List<InviteKeepModel> invitedKeeps = await _invite.GetAllInvitedKeep(userId);
            return new()
            {
                Data = invitedKeeps
            };
        }
        [HttpPost("keepInviteResponse")]
        public async Task<ResponseModel<string>> KeepInviteResponse(InviteResponseModel response)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            bool res = await _invite.ResponseToKeepInvite(response, userId);
            return new()
            {
                Message = $"Inviation {(res ? "Accepted" : "Declied")}"
            };
        }
    }
}
