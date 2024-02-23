namespace Keeper.Common.ViewModels
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public record AddClient(string Name, Guid projectId);
    public record EditClient(Guid Id, string Name, Guid projectId);
}
