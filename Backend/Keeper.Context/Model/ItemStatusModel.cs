using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("ItemStatus")]
    public class ItemStatusModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
