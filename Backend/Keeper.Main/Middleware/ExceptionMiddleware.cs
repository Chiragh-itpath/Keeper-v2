using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using log4net;
using System.Text.Json;

namespace Keeper.Main.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _log;

        public ExceptionMiddleware(RequestDelegate request, ILog log)
        {
            _next = request;
            _log = log;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(InnerException ex)
            {
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new 
                {
                    isSuccess = false,
                    statusName = ex.Status,
                    message = ex.Message
                }));
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new 
                {
                    statusName = StatusType.INTERNAL_SERVER_ERROR,
                    isSuccess = false,
                    message = "Something went Wrong"
                }));
            }
        }
    }
}
