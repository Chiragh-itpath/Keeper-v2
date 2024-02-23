using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Table("Clients")]
    public class ClientModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = default!;
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
