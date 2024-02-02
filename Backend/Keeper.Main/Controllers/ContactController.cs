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
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contact;
        public ContactController(IContactService contact)
        {
            _contact = contact;
        }
        [HttpPost]
        public async Task<ResponseModel<ContactViewModel>> AddContact(AddContact contact)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _contact.AddAsync(contact, userId);
            return new()
            {
                Data = res
            };
        }
        [HttpPut]
        public async Task<ResponseModel<ContactViewModel>> UpdateContact(ContactViewModel contact)
        {
            var res = await _contact.UpdateAsync(contact);
            return new()
            {
                Data = res
            };
        }
        [HttpDelete("{id}")]
        public async Task<ResponseModel<string>> DeleteContact([FromRoute] Guid id)
        {
            await _contact.DeleteAsync(id);
            return new()
            {
                Data = "Deleted"
            };
        }
        [HttpGet]
        public async Task<ResponseModel<List<ContactViewModel>>> GetContacts()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            var res = await _contact.GetAllContacts(userId);
            return new() { Data = res };
        }
    }
}
