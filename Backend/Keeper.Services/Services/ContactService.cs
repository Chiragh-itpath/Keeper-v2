using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepo _contact;
        private readonly IUserService _userService;
        public ContactService(IContactRepo contact, IUserService userService)
        {
            _contact = contact;
            _userService = userService;
        }
        public  ContactViewModel Mapper(ContactModel contact)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                AddedBy = _userService.MapToUserVM(contact.AddedBy),
                AddedPerson= _userService.MapToUserVM(contact.AddedPerson)
            };
        }
        public async Task<List<ContactViewModel>> AddAsync(AddContact contact, Guid userId)
        {
            List<ContactViewModel> contacts = new();
            foreach(var addedId in contact.UserIds)
            {
                var addedContact = await _contact.AddAsync(new ContactModel
                {
                    AddedById = userId,
                    AddedId = addedId
                });
                var viewModel = await GetById(addedContact.Id);
                contacts.Add(viewModel);
            }
            return contacts;
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
