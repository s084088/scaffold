using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiServer.Comm;

/// <summary>
/// 返回数据打包器
/// </summary>
[ApiController]
public class BaseController : Controller
{

    /// <summary>
    /// 返回空或对象
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public WebApiPackage OK(object obj = null)
    {
        return new WebApiPackage
        {
            Success = true,
            ResultCode = 200,
            ResultDesc = "调用成功",
            Result = obj,
        };
    }

    /// <summary>
    /// 返回数据
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public WebApiPackage<T> Data<T>(T result)
    {
        return new WebApiPackage<T>
        {
            Success = true,
            ResultCode = 200,
            ResultDesc = "调用成功",
            Result = result,
        };
    }

    /// <summary>
    /// 返回列表
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public WebApiPackageList<T> List<T>(IEnumerable<T> list, Pagination pagination)
    {
        return new WebApiPackageList<T>
        {
            Success = true,
            ResultCode = 200,
            ResultDesc = "调用成功",
            Result = new WebApiPackageList1<T>
            {
                List = list,
                Pager = pagination,
            }
        };
    }
    /// <summary>
    /// 返回列表和其他数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="pagination"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public WebApiPackageList<T> List<T>(IEnumerable<T> list, Pagination pagination, object data)
    {
        return new WebApiPackageList<T>
        {
            Success = true,
            ResultCode = 200,
            ResultDesc = "调用成功",
            Result = new WebApiPackageList1<T>
            {
                List = list,
                Pager = pagination,
                Data = data
            }
        };
    }

    /// <summary>
    /// 当次请求的临时数据
    /// </summary>
    public RequestTemporaryData RequestData => HttpContext.RequestServices.GetService<RequestTemporaryData>();
}