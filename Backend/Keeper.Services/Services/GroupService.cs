using Keeper.Common.Enums;
using Keeper.Common.Response;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;
using System.Xml.Schema;

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
        public async Task<ResponseModel<GroupViewModel>> AddAsync(AddGroup addGroup, Guid userId)
        {
            var group = await _group.AddAsync(new GroupModel
            {
                Name = addGroup.Name,
                UserId = userId
            });
            await this.AddContacts(new AddContactsToGroup
            {
                GroupId = group.Id,
                ContactIds = addGroup.ContactId
            });
            var result = await _group.GetByIdAsync(group.Id);

            var groupViewModel = await this.FillContacts(result!);
            return new ResponseModel<GroupViewModel>
            {
                IsSuccess = true,
                StatusName = StatusType.SUCCESS,
                Message = "Group Added",
                Data = groupViewModel
            };
        }
        public async Task<ResponseModel<List<GroupViewModel>>> GetAllGroup(Guid userId)
        {
            var groups = await _group.GetAllAsync(userId);
            List<GroupViewModel> groupList = new();
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var viewModel = await this.FillContacts(group);
                groupList.Add(viewModel);
            }
            return new ResponseModel<List<GroupViewModel>>
            {
                IsSuccess = true,
                StatusName = StatusType.SUCCESS,
                Data = groupList
            };
        }

        private async Task<GroupViewModel> FillContacts(GroupModel group)
        {
            var groupViewModel = new GroupViewModel
            {
                Id = group.Id,
                Name = group.Name,
                UserEmail = group.User.Email
            };
            var contacts = new List<ContactViewModel>();

            for (int j = 0; j < group.Linkers?.Count; j++)
            {
                var contact = await _contact.GetById(group.Linkers[j].ContactId);
                contacts.Add(contact);
            }
            groupViewModel.Contacts = contacts;
            return groupViewModel;
        }
        public async Task<ResponseModel<GroupViewModel>> AddContacts(AddContactsToGroup addContacts)
        {
            for (int i = 0; i < addContacts.ContactIds.Count; i++)
            {
                await _linker.AddAsync(new ContactGroupLinkerModel
                {
                    GroupId = addContacts.GroupId,
                    ContactId = addContacts.ContactIds[i]
                });
            }
            var group = await _group.GetByIdAsync(addContacts.GroupId);

            var viewModel = await this.FillContacts(group!);

            return new ResponseModel<GroupViewModel>
            {
                IsSuccess = true,
                StatusName = StatusType.SUCCESS,
                Data = viewModel
            };
        }
    }
}
