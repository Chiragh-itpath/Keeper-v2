using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Contact")]
    public class ContactModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey(nameof(AddedPerson))]
        public Guid AddedId { get; set; }
        public virtual UserModel AddedPerson { get; set; } = default!;
        [ForeignKey(nameof(AddedBy))]
        public Guid AddedById { get; set; }
        public virtual UserModel AddedBy { get; set; } = default!;
    }
}
