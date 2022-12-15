using Microsoft.AspNetCore.Http;

namespace ApiServer.Comm;

public static class HttpContextExtention
{
    public static string GetRemoteHost(this HttpContext httpContext)
    {
        string ip = httpContext.Request.Headers["x-forwarded-for"];
        if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
        {
            ip = httpContext.Request.Headers["Proxy-Client-IP"];
        }
        if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
        {
            ip = httpContext.Request.Headers["WL-Proxy-Client-IP"];
        }
        if (ip == null || ip.Length == 0 || "unknown".Equals(ip))
        {
            ip = httpContext.Connection.RemoteIpAddress.ToString();
        }
        return ip.Equals("0:0:0:0:0:0:0:1") ? "127.0.0.1" : ip;
    }

    public static IActionResult ApiError(string msg = null, int level = 1, string tag = null) => new JsonResult(new WebApiPackage
    {
        Success = false,
        ResultCode = level,
        ResultDesc = msg + tag,
        Result = null,
    });
}