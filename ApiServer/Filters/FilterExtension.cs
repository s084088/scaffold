using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiServer.Filters;

public static class FilterExtension
{
    /// <summary>
    /// 添加自定义拦截器
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static FilterCollection AddCustomFilters(this FilterCollection filters)
    {
        filters.Add<ErrorHandle>();
        filters.Add<ParameterValidationFilter>();
        filters.Add<AuthenticationFilter>();

        return filters;
    }
}