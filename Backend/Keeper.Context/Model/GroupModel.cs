using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Group")]
    public class GroupModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [ForeignKey(nameof(UserModel))]
        public Guid UserId { get; set; }
        public virtual UserModel User { get; set; } = default!;
        public virtual List<ContactGroupLinkerModel> Linkers { get; set; } = default!;
    }
}
