using System;

namespace Cache.Models;

public class CacheApiCount
{
    /// <summary>
    /// 访问地址
    /// </summary>
    public string Api { get; set; }

    /// <summary>
    /// 第一次访问时间
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 总次数
    /// </summary>
    public int SuccessCount { get; set; } = 0;

    /// <summary>
    /// 业务错误次数
    /// </summary>
    public int ErrorCount { get; set; } = 0;

    /// <summary>
    /// 系统错误次数
    /// </summary>
    public int FatalErrorCount { get; set; } = 0;

    /// <summary>
    /// Token失效次数
    /// </summary>
    public int TokenFailCount { get; set; } = 0;

    /// <summary>
    /// 重复请求被拦截的次数
    /// </summary>
    public int InterceptionCount { get; set; } = 0;

    /// <summary>
    /// 参数错误次数
    /// </summary>
    public int ParameterCount { get; set; } = 0;

    /// <summary>
    /// 总时间
    /// </summary>
    public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// 最长时间
    /// </summary>
    public TimeSpan MaxTime { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// 最短时间
    /// </summary>
    public TimeSpan MinTime { get; set; } = TimeSpan.Zero;
}