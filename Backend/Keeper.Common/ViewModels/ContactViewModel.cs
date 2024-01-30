using Keeper.Common.Enums;

namespace Keeper.Common.ViewModels
{
    public class ContactViewModel
    {
        public Guid Id { get; set; }
        public UserViewModel AddedBy { get; set; } = default!;
        public UserViewModel AddedPerson { get; set; } = default!;
        public List<ProjectViewModel> Projects { get; set; } = new();
    }
    public class AddContact
    {
        public UserViewModel User { get; set; } = default!;
        public Guid? ProjectId { get; set; }
        public Permission Permission { get; set; }
    }
}
