namespace Keeper.Common.ViewModels
{
    public class MoveItemModel
    {
        public Guid ItemId { get; set; }
        public Guid CurrentKeepId { get; set; }

        public Guid TargetKeepId { get; set; }
        public MoveAction Action { get; set; } = MoveAction.Move;

    }

    public enum MoveAction
    {
        Move,
        Copy
    }
}
