using AspNetCoreRateLimit;
using Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ApiServer.Middlewares;

/// <summary>
/// 自定义限流中间件
/// </summary>
public class CustomIpRateLimitMiddleware : RateLimitMiddleware<IpRateLimitProcessor>
{
    public CustomIpRateLimitMiddleware(RequestDelegate next,
        IProcessingStrategy processingStrategy,
        IOptions<IpRateLimitOptions> options,
        IIpPolicyStore policyStore,
        IRateLimitConfiguration config
    ) : base(next, options?.Value, new IpRateLimitProcessor(options?.Value, policyStore, processingStrategy), config) { }

    /// <summary>
    /// 记录日志    限制时返回的内容在AppSettings里面配置,目前配置的错误代码为105
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="identity"></param>
    /// <param name="counter"></param>
    /// <param name="rule"></param>
    protected override void LogBlockedRequest(HttpContext httpContext, ClientRequestIdentity identity, RateLimitCounter counter, RateLimitRule rule)
    {
        RequestTemporaryData _reuqestData = httpContext.RequestServices.GetService<RequestTemporaryData>();

        ApiStatistics.Interception(_reuqestData.UrlPath);
        LogHelper.WriteLog(_reuqestData.LogPrefix, "logs/InterceptionLog", DateTime.Now.ToDefaultDateString() + ".log");
    }
}