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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _group;

        public GroupController(IGroupService group)
        {
            _group = group;
        }
        [HttpPost("")]
        public async Task<ResponseModel<GroupViewModel>> AddGroup(AddGroup group)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);

            var res = await _group.AddAsync(group, userId);
            return new ResponseModel<GroupViewModel> { Data = res };
        }
        [HttpGet("")]
        public async Task<ResponseModel<List<GroupViewModel>>> AllGroups()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _group.GetAllGroup(userId);
            return new ResponseModel<List<GroupViewModel>> { Data = res };
        }
        [HttpPost("AddContacts")]
        public async Task<ResponseModel<GroupViewModel>> AddContacts(AddContactsToGroup addContacts)
        {
            var res = await _group.AddContacts(addContacts);
            return new ResponseModel<GroupViewModel> { Data = res };
        }
        [HttpDelete("RemoveContact/{groupId}/{contactId}")]
        public async Task<ResponseModel<string>> RemoveContact(Guid groupId, Guid contactId)
        {
            await _group.RemoveContact(contactId, groupId);
            return new()
            {
                Data = "Success"
            };
        }
    }
}
