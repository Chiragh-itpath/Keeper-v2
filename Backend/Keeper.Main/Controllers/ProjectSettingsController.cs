using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Keeper.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectSettingsController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IClientSevice _clientSevice;
        public ProjectSettingsController(IStatusService statusService, IClientSevice clientSevice)
        {
            _statusService = statusService;
            _clientSevice = clientSevice;
        }
        [HttpPost("Status")]
        public async Task<ResponseModel<StatusViewModel>> AddStauts(AddStauts addStauts)
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
            await _statusService.UpdateStatusAsync(editStatus, userId);
            return new()
            {

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
        [HttpDelete("Client/{id}")]
        public async Task<ResponseModel<string>> DeleteClient([FromRoute] Guid Id)
        {
            var result = await _clientSevice.DeleteAsync(Id);
            return new()
            {
                Data = $"{result}"
            };
        }
    }
}
