using Keeper.Common.Enums;

namespace Keeper.Common.Response
{
    public class ResponseModel<T> where T : class
    {
        public StatusType StatusName { get; set; } = StatusType.SUCCESS;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = default!;
        public T? Data { get; set; }
    }
}
