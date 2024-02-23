namespace Keeper.Common.ViewModels
{
    public class StatusViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid ProjectId { get; set; }
        public bool IsSystem { get; set; } = false;
    }
    public record AddStauts(string Title, Guid ProjectId);
    public record EditStatus(Guid Id, string Title, Guid ProjectId);

}
