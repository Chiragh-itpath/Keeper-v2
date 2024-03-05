using Keeper.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Items")]
    public class ItemModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? URL { get; set; }
        public ItemType Type { get; set; }
        public string? Number { get; set; } 
        public string? To { get; set; }
        public string? DiscussedBy { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.NEW;
        [ForeignKey("StatusModel")]
        public Guid? StatusId { get; set; }
        public virtual ItemStatusModel? StatusModel { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("KeepId")]
        public Guid KeepId { get; set; }
        public virtual KeepModel Keep { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public virtual UserModel CreatedBy { get; set; } = default!;
        public Guid? UpdatedById { get; set; }
        public virtual UserModel? UpdatedBy { get; set; }

    }
}
