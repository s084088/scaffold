namespace BasicLibrary.Comm;

/// <summary>
/// 错误帮助类
/// </summary>
public static class Error
{
    /// <summary>
    /// API专用异常
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="level"></param>
    /// <exception cref="ApiException"></exception>
    public static ApiException Api(string msg = null, int level = 1)
    {
        return new ApiException(msg, level);
    }

    /// <summary>
    /// 校验为true则抛出指定异常
    /// </summary>
    /// <param name="checkParameter"></param>
    /// <param name="errMsg"></param>
    public static void Check(bool checkParameter, string errMsg)
    {
        if (checkParameter)
        {
            throw new Exception(errMsg);
        }
    }
}