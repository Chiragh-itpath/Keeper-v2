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
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _projectService.SaveAsync(project, userId);
            return new ResponseModel<ProjectViewModel> { Data = res };
        }
        [HttpGet("")]
        public async Task<ResponseModel<List<ProjectViewModel>>> GetAll()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _projectService.GetAllAsync(userId);
            return new ResponseModel<List<ProjectViewModel>> { Data = res };
        }
        [HttpGet("{Id}")]
        public async Task<ResponseModel<ProjectViewModel>> GetById([FromRoute] Guid Id)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _projectService.GetByIdAsync(Id, userId);
            return new ResponseModel<ProjectViewModel> { Data = res };
        }
        [HttpPut]
        public async Task<ResponseModel<ProjectViewModel>> Update(EditProject project)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _projectService.UpdateAsync(project, userId);
            return new ResponseModel<ProjectViewModel> { Data = res };
        }
        [HttpDelete("{id}")]
        public async Task<ResponseModel<string>> Delete(Guid id)
        {
            var res = await _projectService.DeleteByIdAsync(id);
            return new ResponseModel<string>() { Data = res.ToString() };
        }
        [HttpGet("Users/{Id}")]
        public async Task<ResponseModel<List<ProjectUsersViewModel>>> InvitedUsers([FromRoute] Guid Id)
        {
            var users = await _projectService.AllInvitedUsers(Id);
            return new ResponseModel<List<ProjectUsersViewModel>>
            {
                Data = users
            };
        }
        [HttpDelete("Remove/{ShareId}")]
        public async Task<ResponseModel<string>> Remove([FromRoute] Guid ShareId)
        {
            var res = await _inviteService.RemoveFromProject(ShareId);
            return new ResponseModel<string>()
            {
                Data = $"{res}"
            };
        }
    }
}
