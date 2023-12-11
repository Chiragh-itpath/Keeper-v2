using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using System.Text.Json;

namespace Keeper.Main.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate request)
        {
            _next = request;
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
                await Console.Out.WriteLineAsync(ex.Message);
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
