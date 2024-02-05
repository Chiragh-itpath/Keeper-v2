using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Contact")]
    public class ContactModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual UserModel User { get; set; } = default!;

    }
}
