using System.ComponentModel;

namespace Keeper.Common.Enums
{
    public enum ItemStatus
    {
        NEW,
        [Description("Waitng from client")]
        WAITING,
        PENDING,
        FOLLOW_UP,
        COMPLETED,
        RE_OPEN
    }
}
