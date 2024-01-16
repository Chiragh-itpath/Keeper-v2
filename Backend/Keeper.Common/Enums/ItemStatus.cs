using System.ComponentModel;

namespace Keeper.Common.Enums
{
    public enum ItemStatus
    {
        NEW,
        [Description("Waitng from client")]
        PEDNING,
        COMPLETED,
        FOLLOW_UP,
        RE_OPEN
    }
}
