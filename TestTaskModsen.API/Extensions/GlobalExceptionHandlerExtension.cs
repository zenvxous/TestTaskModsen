using TestTaskModsen.API.Middlewares;

namespace TestTaskModsen.API.Extensions;

public static class GlobalExceptionHandlerExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}