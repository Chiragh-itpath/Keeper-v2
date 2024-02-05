namespace Keeper.Common.ViewModels
{
    public class ContactViewModel : AddContact
    {
        public Guid Id { get; set; }
    }
    public class AddContact
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
