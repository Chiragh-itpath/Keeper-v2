using System.Configuration.Assemblies;
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
        private readonly IUserRepo _userRepo;
        private readonly IGroupLinkerRepo _linker;
        public ContactService(IContactRepo contact, IUserRepo userRepo, IGroupLinkerRepo linker)
        {
            _contact = contact;
            _userRepo = userRepo;
            _linker = linker;
        }
        public static ContactViewModel Mapper(ContactModel contact)
        {
            return new()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
            };
        }
        public async Task<ContactViewModel> AddAsync(AddContact addContact, Guid userId)
        {
            if (await _userRepo.GetByEmailAsync(addContact.Email) == null)
                throw new InnerException("Email is not registered", StatusType.EMAIL_NOT_FOUND);
            var addedContact = await _contact.AddAsync(new ContactModel
            {
                FirstName = addContact.FirstName,
                LastName = addContact.LastName,
                Email = addContact.Email,
                UserId = userId
            });
            return Mapper(addedContact);
        }
        public async Task<ContactViewModel> UpdateAsync(ContactViewModel contact)
        {
            if (await _userRepo.GetByEmailAsync(contact.Email) == null)
                throw new InnerException("Email is not registered", StatusType.EMAIL_NOT_FOUND);
            var oldContact = await _contact.GetByIdAsync(contact.Id);
            oldContact.FirstName = contact.FirstName;
            oldContact.LastName = contact.LastName;
            oldContact.Email = contact.Email;
            var newContact = await _contact.UpdateAsync(oldContact);
            return Mapper(newContact);
        }
        public async Task<List<ContactViewModel>> GetAllContacts(Guid userId)
        {
            return (await _contact.GetAllAsync(userId))
                .Select(contact => Mapper(contact))
                .ToList();
        }
        public async Task<ContactViewModel> GetById(Guid id)
        {
            return Mapper(await _contact.GetByIdAsync(id));
        }

        public async Task DeleteAsync(Guid id)
        {
            var linkerList = await _linker.GetListByContactId(id);
            var removalTasks = linkerList.Select(linker => _linker.RemoveAsync(linker));
            await Task.WhenAll(removalTasks);
            await _contact.DeleteAsync(await _contact.GetByIdAsync(id));
        }
    }
}
