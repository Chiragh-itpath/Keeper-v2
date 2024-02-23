using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Keeper.Context.Model
{
    [Keyless]
    [Table("RuleBook")]
    public class RuleBookModel
    {
        public string Text { get; set; } = default!;
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
