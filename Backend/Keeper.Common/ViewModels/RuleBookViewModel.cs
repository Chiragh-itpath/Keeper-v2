namespace Keeper.Common.ViewModels
{
    public class RuleBookViewModel
    {
        public Guid ProjectId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
    public record AddRule(string Text,Guid projectId);
}
