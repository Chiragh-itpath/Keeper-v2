using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Comments")]
    public class CommentModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public virtual UserModel User { get; set; } = default!;
        [ForeignKey("ItemId")]
        public Guid ItemId { get; set; }
        public virtual ItemModel Item { get; set; } = default!;
        public Guid? CommentId { get; set; }
        public CommentModel? Comment { get; set; }
        public virtual List<CommentModel>? Comments { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
