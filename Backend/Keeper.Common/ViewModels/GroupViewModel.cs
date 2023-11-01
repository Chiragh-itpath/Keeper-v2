namespace Keeper.Common.ViewModels
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public List<ContactViewModel> Contacts { get; set; } = default!;
    }
    public class AddGroup
    {
        public string Name { get; set; } = string.Empty;
        public List<Guid> ContactId { get; set; } = default!;
    }
    public class AddContactsToGroup
    {
        public Guid GroupId { get; set; }
        public List<Guid> ContactIds { get; set; } = default!;
    }
}
