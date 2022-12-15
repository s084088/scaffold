using System.Collections.Concurrent;
using System.Reflection;
using Auth;
using Auth.Models;
using Cache;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiServer.Filters;

/// <summary>
/// 权限过滤
/// </summary>
public class AuthenticationFilter : IActionFilter
{
    private readonly static ConcurrentDictionary<string, EnumCustomAuthorize> concurrentDictionary = new();
    private readonly RequestTemporaryData _reuqestData;

    public AuthenticationFilter(RequestTemporaryData data)
    {
        _reuqestData = data;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        EnumCustomAuthorize enumCustomAuthorize = GetEnumCustomAuthorize(_reuqestData.UrlPath, context);

        switch (enumCustomAuthorize)
        {
            case EnumCustomAuthorize.None: break;
            case EnumCustomAuthorize.Normal: NormalAuthorize(_reuqestData.UrlPath, context); break;
        }
    }

    /// <summary>
    /// 判断该接口是否需要验证
    /// </summary>
    /// <param name="path"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    private static EnumCustomAuthorize GetEnumCustomAuthorize(string path, ActionExecutingContext context)
    {
        if (concurrentDictionary.TryGetValue(path, out EnumCustomAuthorize enumCustomAuthorize)) return enumCustomAuthorize;

        CustomAttributeData customAttributeData = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.CustomAttributes.FirstOrDefault(l => l.AttributeType == typeof(CustomAuthorizeAttribute))
            ?? (context.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo.CustomAttributes.FirstOrDefault(l => l.AttributeType == typeof(CustomAuthorizeAttribute));

        enumCustomAuthorize = customAttributeData == null ? EnumCustomAuthorize.Normal : (EnumCustomAuthorize)(customAttributeData.ConstructorArguments[0].Value);

        concurrentDictionary.TryAdd(path, enumCustomAuthorize);
        return enumCustomAuthorize;
    }

    /// <summary>
    /// 正常验证, 只要有Token,且Token有效就可以
    /// </summary>
    /// <param name="path"></param>
    /// <param name="context"></param>
    private void NormalAuthorize(string path, ActionExecutingContext context)
    {
        ValidationResult ret = AuthServer.Default.Check(_reuqestData.Token, out UserModel user);

        if (ret == ValidationResult.Success)
        {
            _reuqestData.User = user;
            return;
        }

        if (ret == ValidationResult.Invalid)
            context.Result = HttpContextExtention.ApiError("Token无效", 103);
        else if (ret == ValidationResult.Expired)
            context.Result = HttpContextExtention.ApiError("Token过期", 104);

        ApiStatistics.Authentication(path);

        LogHelper.WriteLog(_reuqestData.LogPrefix, "logs/Authentication", DateTime.Now.ToDefaultDateString() + ".log");
    }
}

/// <summary>
/// 自定义权限验证
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute
{
    public EnumCustomAuthorize EnumAuthorizeAttribute { get; set; }

    /// <summary>
    /// 自定义权限验证
    /// </summary>
    /// <param name="enumAuthorizeAttribute">验证方式</param>
    public CustomAuthorizeAttribute(EnumCustomAuthorize enumAuthorizeAttribute = EnumCustomAuthorize.Normal)
    {
        EnumAuthorizeAttribute = enumAuthorizeAttribute;
    }
}

/// <summary>
/// 自定义权限验证枚举
/// </summary>
public enum EnumCustomAuthorize
{
    /// <summary>
    /// 免验证
    /// </summary>
    None = 0,

    /// <summary>
    /// 正常验证, 只要有Token,且Token有效就可以
    /// </summary>
    Normal = 1,
}