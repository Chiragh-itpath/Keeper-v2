using Keeper.Common.Enums;

namespace Keeper.Common.ViewModels
{
    public class AddKeep 
    {
        public string Title { get; set; } = default!;
        public Guid ProjectId { get; set; }
        public string? Tag { get; set; }
    }
    public class EditKeep : AddKeep
    {
        public Guid Id { get; set; }
    }
    public class KeepViewModel : AddKeep
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = default!;
        public string? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<KeepUserViewModel> Users { get; set; } = new();
    }
    public class KeepUserViewModel
    {
        public Guid ShareId { get; set; }
        public bool IsAccepted { get; set; }
        public Permission Permission { get; set; }
        public UserViewModel InvitedUser { get; set; } = new();
    }
}
