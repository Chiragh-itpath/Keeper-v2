using Keeper.Common.Enums;
using Keeper.Common.Response;
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
        private async Task<ContactViewModel> ContactViewModelConvertor(ContactModel contact)
        {
            var user = await _user.GetByEmailAsync(contact.Email);
            return new ContactViewModel
            {
                Id = contact.Id,
                Email = contact.Email,
                UserId = user!.Id,
                Contact = user!.Contact,
                UserName = user!.UserName
            };
        }
        public async Task<ResponseModel<ContactViewModel>> AddAsync(AddContact contact, Guid userId)
        {
            var res = await _contact.FindByEmailAsync(contact.Email, userId);
            if (res != null)
            {
                return new ResponseModel<ContactViewModel>
                {
                    IsSuccess = false,
                    StatusName = StatusType.ALREADY_EXISTS,
                    Message = "Contact already exist"
                };
            }
            var newContact = await _contact.AddAsync(new ContactModel
            {
                Email = contact.Email,
                UserId = userId,
            });
            var viewModel = await ContactViewModelConvertor(newContact);
            return new ResponseModel<ContactViewModel>
            {
                IsSuccess = true,
                StatusName = StatusType.SUCCESS,
                Message = "Contact Added",
                Data = viewModel
            };
        }

        public async Task<ResponseModel<List<ContactViewModel>>> GetAllContacts(Guid userId)
        {
            var res = await _contact.GetAllAsync(userId);
            List<ContactViewModel> contacts = new();
            for (int i = 0; i < res.Count; i++)
            {
                var contact = await ContactViewModelConvertor(res[i]);
                contacts.Add(contact);
            }
            return new ResponseModel<List<ContactViewModel>>
            {
                IsSuccess = true,
                StatusName = StatusType.SUCCESS,
                Data = contacts
            };
        }
    }
}
