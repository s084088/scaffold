using Util;

namespace Dto.Comm;

/// <summary>
/// Web返回基础包
/// </summary>
public class WebApiPackage
{
    /// <summary>
    /// 是否成功  true 或false
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 返回代码 200 成功 101 错误 102 验证错误 103 Token无效  104 Token过期
    /// </summary>
    public int ResultCode { get; set; }
    /// <summary>
    /// 返回结果描述
    /// </summary>
    public string ResultDesc { get; set; }
    /// <summary>
    /// 返回结果
    /// </summary>
    public object Result { get; set; }
}

/// <summary>
/// Web返回基础包
/// </summary>
/// <typeparam name="T"></typeparam>
public class WebApiPackage<T>
{
    /// <summary>
    /// 是否成功  true 或false
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 返回代码 200 成功 101 错误 102 验证错误
    /// </summary>
    public int ResultCode { get; set; }
    /// <summary>
    /// 返回结果描述
    /// </summary>
    public string ResultDesc { get; set; }
    /// <summary>
    /// 返回结果
    /// </summary>
    public T Result { get; set; }
}

/// <summary>
/// Web返回列表包
/// </summary>
/// <typeparam name="T"></typeparam>
public class WebApiPackageList<T>
{
    public bool Success { get; set; }

    public int ResultCode { get; set; }

    public string ResultDesc { get; set; }

    public WebApiPackageList1<T> Result { get; set; }
}

/// <summary>
/// 列表结构
/// </summary>
/// <typeparam name="T"></typeparam>
public class WebApiPackageList1<T>
{
    public IEnumerable<T> List { get; set; }

    public Pagination Pager { get; set; }

    public object Data { get; set; } = null;
}

