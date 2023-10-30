﻿using Keeper.Common.Response;
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
        [HttpPost("")]
        public async Task<ResponseModel<ContactViewModel>> AddContact(AddContact contact)
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            return await _contact.AddAsync(contact, userId);
        }
        [HttpGet("")]
        public async Task<ResponseModel<List<ContactViewModel>>> GetContacts()
        {
            var user = User.Identities.First();
            var claims = user.Claims.ToList();
            var userId = Guid.Parse(claims.ElementAt(3).Value);
            return await _contact.GetAllContacts(userId);
        }

    }
}
