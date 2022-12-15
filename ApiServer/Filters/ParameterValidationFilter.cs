using System.Text;
using Cache;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiServer.Filters;

/// <summary>
/// 参数验证过滤器
/// </summary>
public class ParameterValidationFilter : IActionFilter
{
    private readonly RequestTemporaryData _reuqestData;


    public ParameterValidationFilter(RequestTemporaryData data)
    {
        _reuqestData = data;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //参数验证
        if (context.ModelState.IsValid == false)
        {
            context.Result = HttpContextExtention.ApiError(ValidErr(context.ModelState), 106);

            ApiStatistics.Parameter(_reuqestData.UrlPath);
            LogHelper.WriteLog(_reuqestData.LogPrefix, "logs/ParameterLog", DateTime.Now.ToDefaultDateString() + ".log");
            return;
        }
    }

    /// <summary>
    /// 返回验证报错
    /// </summary>
    /// <param name="modelstate"></param>
    /// <returns></returns>
    public static string ValidErr(ModelStateDictionary modelstate)
    {
        StringBuilder sb = new();
        foreach (string propName in modelstate.Keys)//遍历每个属性
        {
            //如果这个属性的错误消息<=0 那么表示数据没有错误
            if (modelstate[propName].Errors.Count <= 0)
            {
                continue;
            }
            //一个属性可能有多个错误，那么就对这个属性的错误进行遍历
            foreach (ModelError error in modelstate[propName].Errors)
            {
                sb.Append(propName + "属性验证错误：" + error.ErrorMessage);//将错误信息添加到sb中
                sb.AppendLine();
            }
        }
        return sb.ToString();
    }
}