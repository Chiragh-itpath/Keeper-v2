using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            await _invite.InviteToProjectAsync(invite, userId);
            return new ResponseModel<string>
            {
                Message = "Invited"
            };
        }
        [HttpGet("AllInvitedProjects")]
        public async Task<ResponseModel<List<InvitedProjectModel>>> InvitedProjects()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _invite.GetAllInvitedProject(userId);
            return new ResponseModel<List<InvitedProjectModel>>
            {
                Data = res
            };
        }
        [HttpPost("ProjectInviteResponse")]
        public async Task<ResponseModel<string>> ProjectInviteResponse(InviteResponseModel response)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _invite.ResponseToProjectInvite(response, userId);
            return new ResponseModel<string>
            {
                Message = $"Inviation {(res ? "Accepted" : "Declied")}"
            };
        }
        [HttpPost("InviteToKeep")]
        public async Task<ResponseModel<string>> InviteToKeep(KeepInviteModel invite)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            await _invite.InviteToKeepAsync(invite, userId);
            return new ResponseModel<string>
            {
                Message = "Invited"
            };
        }
        [HttpGet("InvitedKeeps")]
        public async Task<ResponseModel<List<InviteKeepModel>>> InvitedKeeps()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _invite.GetAllInvitedKeep(userId);
            return new ResponseModel<List<InviteKeepModel>>
            {
                Data = res
            };
        }
        [HttpPost("keepInviteResponse")]
        public async Task<ResponseModel<string>> KeepInviteResponse(InviteResponseModel response)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _invite.ResponseToKeepInvite(response, userId);
            return new ResponseModel<string>
            {
                Message = $"Inviation {(res ? "Accepted" : "Declied")}"
            };
        }
        [HttpGet("ProjectCollaborators")]
        public async Task<ResponseModel<List<Collaborator>>> ProjectCollaborators(Guid ProjectId)
        {
            var res = await _invite.GetProjectCollaborators(ProjectId);
            return new ResponseModel<List<Collaborator>>
            {
                Data = res
            };
        }
        [HttpGet("KeepCollaborators")]
        public async Task<ResponseModel<List<Collaborator>>> KeepCollaborators(Guid keepId)
        {
            var res = await _invite.GetKeepCollaborators(keepId);
            return new ResponseModel<List<Collaborator>>
            {
                Data = res
            };
        }
    }
}
