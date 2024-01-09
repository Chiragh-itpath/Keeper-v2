using Keeper.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("SharedKeeps")]
    public class SharedKeepsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public virtual UserModel User { get; set; } = default!;
        [ForeignKey("KeepId")]
        public Guid KeepId { get; set; }
        public virtual KeepModel Keep { get; set; } = default!;
        public Guid ProjectId { get; set; }
        public virtual ProjectModel Project { get; set; } = default!;
        public bool IsAccepted { get; set; } = false;
        public Permission Permission { get; set; } = Permission.VIEW;
    }
}
