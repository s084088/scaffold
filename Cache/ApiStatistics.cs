using System;
using System.Collections.Concurrent;
using System.Linq;
using Cache.Models;

namespace Cache;

public static class ApiStatistics
{
    /// <summary>
    /// 所有接口
    /// </summary>
    public static ConcurrentQueue<CacheApiCount> AllApi { get; } = new ConcurrentQueue<CacheApiCount>();


    private static CacheApiCount GetCacheAPICount(string apiUrl)
    {
        CacheApiCount cacheAPICount = AllApi.FirstOrDefault(l => l.Api == apiUrl);
        if (cacheAPICount == null)
        {
            cacheAPICount = new CacheApiCount { Api = apiUrl };
            AllApi.Enqueue(cacheAPICount);
        }
        return cacheAPICount;
    }

    /// <summary>
    /// 访问次数增加
    /// </summary>
    /// <param name="pathlow"></param>
    /// <param name="ts"></param>
    public static void StatisticsAccess(string apiUrl, TimeSpan ts)
    {
        CacheApiCount cacheAPICount = GetCacheAPICount(apiUrl);
        cacheAPICount.SuccessCount++;
        cacheAPICount.TotalTime += ts;
        if (ts > cacheAPICount.MaxTime)
            cacheAPICount.MaxTime = ts;
        if (ts < cacheAPICount.MinTime || cacheAPICount.MinTime == TimeSpan.Zero)
            cacheAPICount.MinTime = ts;
    }

    /// <summary>
    /// 限流拦截增加
    /// </summary>
    /// <param name="apiUrl"></param>
    public static void Interception(string apiUrl) => GetCacheAPICount(apiUrl).InterceptionCount++;

    /// <summary>
    /// 参数错误增加
    /// </summary>
    /// <param name="apiUrl"></param>
    public static void Parameter(string apiUrl) => GetCacheAPICount(apiUrl).ParameterCount++;

    /// <summary>
    /// Token验证失败增加
    /// </summary>
    /// <param name="apiUrl"></param>
    public static void Authentication(string apiUrl) => GetCacheAPICount(apiUrl).TokenFailCount++;

    /// <summary>
    /// 业务错误失败增加
    /// </summary>
    /// <param name="apiUrl"></param>
    public static void Error(string apiUrl) => GetCacheAPICount(apiUrl).ErrorCount++;

    /// <summary>
    /// 系统错误失败增加
    /// </summary>
    /// <param name="apiUrl"></param>
    public static void FatalError(string apiUrl) => GetCacheAPICount(apiUrl).FatalErrorCount++;
}