using Application.Common.Exceptions;
using Newtonsoft.Json;

namespace WebApi.Middlewares.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (AbstractHttpException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new { Message = ex.Message }
                ));
        }
    }
}
