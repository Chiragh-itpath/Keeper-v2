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
        private readonly IInviteService _inviteService;
        private readonly IProjectService _projectService;
        private readonly IProjectShareRepo _projectShareRepo;
        public ContactService(IContactRepo contact, IUserService userService, IInviteService inviteService, IProjectService projectService, IProjectShareRepo projectShareRepo)
        {
            _contact = contact;
            _userService = userService;
            _inviteService = inviteService;
            _projectService = projectService;
            _projectShareRepo = projectShareRepo;
        }
        public ContactViewModel Mapper(ContactModel contact)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                AddedBy = _userService.MapToUserVM(contact.AddedBy),
                AddedPerson = _userService.MapToUserVM(contact.AddedPerson)
            };
        }
        public async Task<ContactViewModel> AddAsync(AddContact addContact, Guid userId)
        {
            var addedContact = await _contact.AddAsync(new ContactModel
            {
                AddedById = userId,
                AddedId = addContact.User.Id
            });
            if (addContact.ProjectId != null)
            {
                await _inviteService.InviteToProjectAsync(new ProjectInviteModel
                {
                    User = addContact.User,
                    ProjectId = (Guid)addContact.ProjectId,
                    Permission = addContact.Permission
                }, userId);
            }
            return (await GetById(addedContact.Id));
        }

        public async Task<List<ContactViewModel>> GetAllContacts(Guid userId)
        {
            var contactList = await _contact.GetAllAsync(userId);
            return contactList
                .Select(contact => Mapper(contact))
                .ToList();
        }
        public async Task<ContactViewModel> GetById(Guid id)
        {
            return Mapper(await _contact.GetByIdAsync(id));
        }
    }
}
