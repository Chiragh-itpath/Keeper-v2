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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IInviteService _inviteService;

        public ProjectController(IProjectService projectService, IInviteService inviteService)
        {
            _projectService = projectService;
            _inviteService = inviteService;
        }

        [HttpPost("")]
        public async Task<ResponseModel<ProjectViewModel>> Post(AddProject project)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            ProjectViewModel res = await _projectService.SaveAsync(project, userId);
            return new() { Data = res };
        }

        [HttpGet("")]
        public async Task<ResponseModel<List<ProjectViewModel>>> GetAll()
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            List<ProjectViewModel> res = await _projectService.GetAllAsync(userId);
            return new() { Data = res };
        }

        [HttpGet("{Id}")]
        public async Task<ResponseModel<ProjectViewModel>> GetById([FromRoute] Guid Id)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            ProjectViewModel res = await _projectService.GetByIdAsync(Id, userId);
            return new() { Data = res };
        }

        [HttpPut]
        public async Task<ResponseModel<ProjectViewModel>> Update(EditProject project)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            ProjectViewModel res = await _projectService.UpdateAsync(project, userId);
            return new() { Data = res };
        }

        [HttpDelete("{id}")]
        public async Task<ResponseModel<string>> Delete(Guid id)
        {
            bool res = await _projectService.DeleteByIdAsync(id);
            return new() { Data = res.ToString() };
        }

        [HttpGet("Users/{Id}")]
        public async Task<ResponseModel<List<ProjectUsersViewModel>>> InvitedUsers([FromRoute] Guid Id)
        {
            List<ProjectUsersViewModel> users = await _projectService.AllInvitedUsers(Id);
            return new() { Data = users };
        }

        [HttpDelete("Remove/{ShareId}")]
        public async Task<ResponseModel<string>> Remove([FromRoute] Guid ShareId)
        {
            int res = await _inviteService.RemoveFromProject(ShareId);
            return new() { Data = $"{res}" };
        }
        [HttpPut("UpdatePermission")]
        public async Task<ResponseModel<string>> UpdatePermission(List<UpdatePermission> updateModel)
        {
            await _inviteService.UpdatePermissionOnProject(updateModel);
            return new()
            {
                Message = "Permission updated",
                Data = "success"
            };
        }
    }
}
