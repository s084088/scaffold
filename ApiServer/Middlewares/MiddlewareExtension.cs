using Microsoft.AspNetCore.Builder;

namespace ApiServer.Middlewares;

public static class MiddlewareExtension
{
    /// <summary>
    /// 添加自定义中间件
    /// </summary>
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CreateTempDataMiddleware>()
                      .UseMiddleware<StatisticsAccessCountMiddleware>()
                      .UseMiddleware<CustomIpRateLimitMiddleware>();
    }
}