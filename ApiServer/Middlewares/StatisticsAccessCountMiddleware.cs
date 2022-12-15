using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Middlewares;

/// <summary>
/// 访问次数统计
/// </summary>
public class StatisticsAccessCountMiddleware
{
    private readonly RequestDelegate _next;

    public StatisticsAccessCountMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        RequestTemporaryData _reuqestData = httpContext.RequestServices.GetService<RequestTemporaryData>();

        long mem = Process.GetCurrentProcess().WorkingSet64;

        httpContext.Request.EnableBuffering();
        using StreamReader reader = new(httpContext.Request.Body);
        string body = reader.ReadToEndAsync().Result.Replace('\r',' ').Replace('\n',' ').Replace('\t',' ');
        httpContext.Request.Body.Position = 0;

        Stream stream = httpContext.Response.Body;
        using MemoryStream ms = new();
        httpContext.Response.Body = ms;

        await _next(httpContext);

        ms.Position = 0;
        using StreamReader reader1 = new(ms);
        string body1 = reader1.ReadToEndAsync().Result.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
        ms.Position = 0;
        await ms.CopyToAsync(stream);
        httpContext.Response.Body = stream;

        TimeSpan elapsed = DateTime.Now - _reuqestData.CreateTime;

        ApiStatistics.StatisticsAccess(_reuqestData.UrlPath, elapsed);
        LogHelper.WriteLog($"{_reuqestData.LogPrefix}Mem:{mem} \t Time:{elapsed.TotalSeconds} \t Request:{body} \t Response:{body1}", "logs/AccessLog", _reuqestData.Data + ".log");

        if (elapsed.TotalSeconds > 1000)
        {
            LogHelper.WriteLog($"{_reuqestData.LogPrefix}Mem:{mem} \t Time:{elapsed.TotalSeconds} \t Request:{body} \t Response:{body1}", "logs/AccessLog", _reuqestData.Data + "_1000.log");
        }
    }
}