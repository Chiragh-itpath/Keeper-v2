using System.ComponentModel;

namespace Keeper.Common.Enums
{
    public enum ItemType
    {
        TICKET,
        [Description("Pull Request")]
        PR,
        MAIL,
        SUMMARY_MAIL,
        CUSTOM
    }
}
