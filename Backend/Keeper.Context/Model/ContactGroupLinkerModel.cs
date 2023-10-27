using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("ContactGroupLinker")]
    public class ContactGroupLinkerModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey(nameof(ContactModel))]
        public Guid ContactId { get; set; }
        public virtual ContactModel Contact { get; set; } = default!;
        [ForeignKey(nameof(GroupModel))]
        public Guid GroupId { get; set; }
        public virtual GroupModel Group { get; set; } = default!;
    }
}
