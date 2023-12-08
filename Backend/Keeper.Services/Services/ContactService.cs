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
        private static ContactViewModel Mapper(ContactModel contact)
        {
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
            return Mapper(newContact);
        }

        public async Task<List<ContactViewModel>> GetAllContacts(Guid userId)
        {
            var contacts = await _contact.GetAllAsync(userId);
            return contacts.Select(contact => Mapper(contact)).ToList();
        }
        public async Task<ContactViewModel> GetById(Guid id)
        {
            return Mapper(await _contact.GetByIdAsync(id));
        }
    }
}
