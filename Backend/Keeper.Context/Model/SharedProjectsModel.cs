using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("SharedProjects")]
    public class SharedProjectsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId {  get; set; }
        public virtual UserModel User { get; set; } = default!;
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public virtual ProjectModel Project { get; set; } = default!;
        public bool IsAccepted { get; set; } = false;
    }
}
