using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Interfaces;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepo _contact;
        private readonly IUserRepo _user;
        public ContactService(IContactRepo contact, IUserRepo user)
        {
            _contact = contact;
            _user = user;
        }
        private async Task<ContactViewModel> Mapper(ContactModel contact)
        {
            //var user = await _user.GetByEmailAsync(contact.Email);
            return new ContactViewModel
            {
                Id = contact.Id,
                Email = contact.Email,
                UserId = contact.UserId,
                Contact = contact.User.Contact,
                UserName = contact.User.UserName
            };
        }
        public async Task<ContactViewModel> AddAsync(AddContact contact, Guid userId)
        {
            var res = await _contact.FindByEmailAsync(contact.Email, userId);
            if (res != null)
            {
                throw new InnerException("Contact Already Exist",StatusType.ALREADY_EXISTS);
            }
            var newContact = await _contact.AddAsync(new ContactModel
            {
                Email = contact.Email,
                UserId = userId,
            });
            return await Mapper(newContact);
        }

        public async Task<List<ContactViewModel>> GetAllContacts(Guid userId)
        {
            var contacts = await _contact.GetAllAsync(userId);
            var task = contacts.Select(async contact => await Mapper(contact)).ToList();
            var contactArray = await Task.WhenAll(task);
            return contactArray.ToList();
        }
        public async Task<ContactViewModel> GetById(Guid id)
        {
            var contact = await _contact.GetByIdAsync(id);
            return await Mapper(contact);
        }
    }
}
