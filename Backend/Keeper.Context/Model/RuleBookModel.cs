using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("RuleBook")]
    public class RuleBookModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
