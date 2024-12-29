using TestTaskModsen.API.Middlewares;

namespace TestTaskModsen.API.Extensions;

public static class ExceptionExtension
{
    public static IApplicationBuilder UseException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}