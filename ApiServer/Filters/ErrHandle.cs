using Cache;
using Microsoft.AspNetCore.Mvc.Filters;
using Util.Model;

namespace ApiServer.Filters;

/// <summary>
/// 接口错误统一处理
/// </summary>
public class ErrorHandle : ExceptionFilterAttribute
{
    private readonly RequestTemporaryData _reuqestData;

    public ErrorHandle(RequestTemporaryData data)
    {
        _reuqestData = data;
    }

    public override void OnException(ExceptionContext context)
    {
        string message = context.Exception.Message;
        string detail = context.Exception.ToString();

        //异常分类
        switch (context.Exception)
        {
            case ApiException a:
                context.Result = HttpContextExtention.ApiError(a.Message, a.Level, a.Tag);

                ApiStatistics.Error(_reuqestData.UrlPath);
                LogHelper.WriteLog($"{_reuqestData.LogPrefix}\r\nMessage:{message}\r\nDetail:{detail}", "logs/AccessErrorLog", DateTime.Now.ToDefaultDateString() + ".log");
                break;

            default:
                context.Result = HttpContextExtention.ApiError("系统繁忙", 9, context.Exception.Message);

                ApiStatistics.FatalError(_reuqestData.UrlPath);
                LogHelper.WriteLog($"{_reuqestData.LogPrefix}\r\nMessage:{message}\r\nDetail:{detail}", "logs/AccessErrorLog", DateTime.Now.ToDefaultDateString() + "_Fatal.log");
                break;
        }
    }
}