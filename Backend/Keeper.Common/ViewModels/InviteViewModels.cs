using Keeper.Common.Enums;

namespace Keeper.Common.ViewModels
{
    public class ProjectInviteModel 
    {
        public Guid ProjectId { get; set; }
        public UserViewModel User { get; set; } = default!;
        public Permission Permission { get; set; }
    }
    
    public class KeepInviteModel : ProjectInviteModel
    {
        public Guid KeepId { get; set; }
    }
    public class InviteResponseModel
    {
        public Guid InviteId { get; set; }
        public bool Response { get; set; }
    }
    public class CommonInvitedModel
    {
        public Guid InviteId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
    public class InvitedProjectModel : CommonInvitedModel
    {
        public Guid ProjectId { get; set; }
    }
    public class InviteKeepModel : InvitedProjectModel
    {
        public Guid KeepId { get; set; }
    }
    public class UpdatePermission
    {
        public Guid ShareId { get; set; }
        public Permission Permission { get; set; }
    }
}
