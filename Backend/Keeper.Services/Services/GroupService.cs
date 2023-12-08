using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;

namespace Keeper.Services.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepo _group;
        private readonly IContactService _contact;
        private readonly IGroupLinkerRepo _linker;
        public GroupService(IGroupRepo group, IGroupLinkerRepo linker, IContactService contact)
        {
            _group = group;
            _linker = linker;
            _contact = contact;
        }
        public async Task<GroupViewModel> AddAsync(AddGroup addGroup, Guid userId)
        {
            var group = await _group.AddAsync(new GroupModel
            {
                Name = addGroup.Name,
                UserId = userId
            });
            await AddContacts(new AddContactsToGroup
            {
                GroupId = group.Id,
                ContactIds = addGroup.ContactId
            });
            var result = await _group.GetByIdAsync(group.Id);

            var groupViewModel = await FillContacts(result!);
            return groupViewModel;
        }
        public async Task<List<GroupViewModel>> GetAllGroup(Guid userId)
        {
            var groups = await _group.GetAllAsync(userId);
            List<GroupViewModel> viewModels = new();
            foreach (var group in groups)
            {
                viewModels.Add(await FillContacts(group));
            }
            return viewModels;
        }
        private async Task<GroupViewModel> FillContacts(GroupModel group)
        {
            var groupViewModel = new GroupViewModel
            {
                Id = group.Id,
                Name = group.Name,
                UserEmail = group.User.Email,
                Contacts = new()
            };
            foreach(var linker in group.Linkers)
            {
                groupViewModel.Contacts.Add(await _contact.GetById(linker.ContactId));
            }
            return groupViewModel;
        }
        public async Task<GroupViewModel> AddContacts(AddContactsToGroup addContacts)
        {
            var addTasks = addContacts.ContactIds.Select(contactId =>
                _linker.AddAsync(new ContactGroupLinkerModel
                {
                    GroupId = addContacts.GroupId,
                    ContactId = contactId
                }));

            await Task.WhenAll(addTasks);
            var group = await _group.GetByIdAsync(addContacts.GroupId);
            var viewModel = await FillContacts(group!);
            return viewModel;
        }

    }
}
