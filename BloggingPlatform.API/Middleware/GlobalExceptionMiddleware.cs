using System.Text.Json;
using BloggingPlatform.Application.Common;
using BloggingPlatform.Application.DTOs.Common;

namespace BloggingPlatform.API.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException ex)
        {
            logger.LogWarning(ex, "Business exception at {Path}", context.Request.Path);
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(ex.Message, ex.Errors.ToArray());
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail("Unexpected error", "Please contact support");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
