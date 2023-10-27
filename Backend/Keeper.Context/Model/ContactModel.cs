using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Contact")]
    public class ContactModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey(nameof(UserModel))]
        public Guid UserId { get; set; }
        public virtual UserModel User { get; set; } = default!;
    }
}
