namespace Keeper.Common.ViewModels
{
    public class AddComment
    {
        public string Content { get; set; } = string.Empty;
        public Guid ItemId { get; set; }
        public Guid? CommentId { get; set; } = null;
    }
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime Date {  get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
    }
}
