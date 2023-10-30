namespace Keeper.Common.ViewModels
{
    public class ContactViewModel : UserViewModel
    {
        public Guid UserId { get; set; }
    }
    public class AddContact
    {
        public string Email { get; set; } = string.Empty;
    }
}
