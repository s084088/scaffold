using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Middlewares;

public class CreateTempDataMiddleware
{
    private readonly RequestDelegate _next;

    public CreateTempDataMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext httpContext)
    {
        RequestTemporaryData _reuqestData = httpContext.RequestServices.GetService<RequestTemporaryData>();

        _reuqestData.UrlPath = httpContext.Request.Path.Value.ToLower();
        _reuqestData.RemoteAddress = httpContext.GetRemoteHost();
        _reuqestData.Token = httpContext.Request.Headers["token"];
        _reuqestData.DeviceId = httpContext.Request.Headers["deviceid"];

        await _next(httpContext);
    }
}