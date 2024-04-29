using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Keeper.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectSettingsController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IClientSevice _clientSevice;
        private readonly IRuleBookService _ruleBookService;
        public ProjectSettingsController(IStatusService statusService, IClientSevice clientSevice, IRuleBookService ruleBookService)
        {
            _statusService = statusService;
            _clientSevice = clientSevice;
            _ruleBookService = ruleBookService;
        }
        [HttpPost("Status")]
        public async Task<ResponseModel<StatusViewModel>> AddStauts([FromBody] AddStauts addStauts)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            var status = await _statusService.AddStatusAsync(addStauts, userId);
            return new()
            {
                Data = status
            };
        }
        [HttpPut("Status")]
        public async Task<ResponseModel<StatusViewModel>> UpdateStauts(EditStatus editStatus)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            var status = await _statusService.UpdateStatusAsync(editStatus, userId);
            return new()
            {
                Data = status
            };
        }
        [HttpGet("Status/{ProjectId}")]
        public async Task<ResponseModel<List<StatusViewModel>>> GetAllStatus([FromRoute] Guid ProjectId)
        {
            var statusList = await _statusService.GetAll(ProjectId, IncludeSystem: true);
            return new()
            {
                Data = statusList
            };
        }
        [HttpDelete("Status/{id}")]
        public async Task<ResponseModel<string>> DeleteStatus([FromRoute] Guid Id)
        {
            var result = await _statusService.DeleteStatusAsync(Id);
            return new()
            {
                Data = $"{result}"
            };
        }
        [HttpPost("Client")]
        public async Task<ResponseModel<ClientViewModel>> AddClient(AddClient addClient)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            var client = await _clientSevice.AddAsync(addClient, userId);
            return new()
            {
                Data = client
            };
        }
        [HttpPut("Client")]
        public async Task<ResponseModel<ClientViewModel>> UpdateClient(EditClient editClient)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            var client = await _clientSevice.UpdateAsync(editClient, userId);
            return new()
            {
                Data = client
            };
        }
        [HttpGet("Client/{ProjectId}")]
        public async Task<ResponseModel<List<ClientViewModel>>> GetAllClient([FromRoute] Guid ProjectId)
        {
            var clientList = await _clientSevice.GetAllAsync(ProjectId);
            return new()
            {
                Data = clientList
            };
        }

        [HttpDelete("Client/{id}")]
        public async Task<ResponseModel<string>> DeleteClient([FromRoute] Guid Id)
        {
            var result = await _clientSevice.DeleteAsync(Id);
            return new()
            {
                Data = $"{result}"
            };
        }
        [HttpPut("RuleBook")]
        public async Task<ResponseModel<RuleBookViewModel>> UpdateRule(AddRule rule)
        {
            ClaimsIdentity user = User.Identities.First();
            List<Claim> claims = user.Claims.ToList();
            Guid userId = Guid.Parse(claims.ElementAt(3).Value);
            var data = await _ruleBookService.AddOrUpdateAsync(rule, userId);
            return new() { Data = data };
        }
        [HttpGet("RuleBook/{ProjectId}")]
        public async Task<ResponseModel<RuleBookViewModel>> GetRule([FromRoute] Guid ProjectId)
        {
            var rule = await _ruleBookService.GetByProjectId(ProjectId);

            return new() { Data = rule };
        }

    }
}
