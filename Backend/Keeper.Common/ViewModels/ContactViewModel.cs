namespace Keeper.Common.ViewModels
{
    public class ContactViewModel 
    {
        public Guid Id { get; set; }
        public UserViewModel AddedBy { get; set; } = default!;
        public UserViewModel AddedPerson { get; set; } = default!;
    }
    public class AddContact
    {
        public List<Guid> UserIds { get; set; } = default!;
    }
}
