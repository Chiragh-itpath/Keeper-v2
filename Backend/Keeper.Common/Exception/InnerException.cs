using Keeper.Common.Enums;

namespace Keeper.Common.InnerException
{
    public class InnerException : Exception
    {
        public StatusType Status;
        public InnerException(string message, StatusType _status) : base(message)
        {
            Status = _status;
        }
    }
}
