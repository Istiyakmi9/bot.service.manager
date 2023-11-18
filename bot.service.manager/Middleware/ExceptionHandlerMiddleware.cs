using bot.service.manager.Model;
using Newtonsoft.Json;
using System.Net;

namespace Bot.Service.Manager.MiddlewareServices
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public static bool LoggingFlag = false;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.OK;
            var result = JsonConvert.SerializeObject(new ApiResponse
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                HttpStatusMessage = e.Message,
                ResponseBody = new { e.Message, InnerMessage = e.InnerException?.Message }
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
