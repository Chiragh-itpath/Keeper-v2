using System.ComponentModel;

namespace Keeper.Common.Enums
{
    public enum StatusType
    {
        [Description("Success")]
        SUCCESS,
        [Description("Already Exists")]
        ALREADY_EXISTS,
        [Description("Not Found")]
        NOT_FOUND,
        [Description("Not Authorised")]
        UNAUTHORISED,
        [Description("Not Valid")]
        NOT_VALID,
        [Description("Email Not Found")]
        EMAIL_NOT_FOUND,
        [Description("Email Already Exists")]
        EMAIL_EXISTS,
        [Description("Password not mathced")]
        PASSWORD_NOT_MATCHED,
        [Description("Internal Server Error")]
        INTERNAL_SERVER_ERROR
    }
}
