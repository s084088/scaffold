using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Util.Model;

namespace Util;

/// <summary>
/// 系统缓存帮助类
/// </summary>
public class SystemCache : ICache
{
    static SystemCache()
    {
        var provider = new ServiceCollection().AddMemoryCache().BuildServiceProvider();
        Cache = provider.GetService<IMemoryCache>();
    }

    private static IMemoryCache Cache { get; }

    public bool ContainsKey(string key)
    {
        return Cache.TryGetValue(key, out object _);
    }

    public object GetCache(string key)
    {
        return Cache.Get(key);
    }

    public T GetCache<T>(string key) where T : class
    {
        return (T)GetCache(key);
    }

    public void RemoveCache(string key)
    {
        Cache.Remove(key);
    }

    public void SetCache(string key, object value)
    {
        Cache.Set(key, value);
    }

    public void SetCache(string key, object value, TimeSpan timeout)
    {
        Cache.Set(key, value, new DateTimeOffset(DateTime.Now.ToCstTime() + timeout));
    }

    public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType)
    {
        if (expireType == ExpireType.Absolute)
            Cache.Set(key, value, new DateTimeOffset(DateTime.Now.ToCstTime() + timeout));
        else
            Cache.Set(key, value, timeout);
    }

    public void SetKeyExpire(string key, TimeSpan expire)
    {
        var value = GetCache(key);
        SetCache(key, value, expire);
    }
    /// <summary>
    /// 消息添加到队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="subscriberName"></param>
    /// <param name="data"></param>
    public void PutMq<T>(string subscriberName, T data)
    {
        throw new ApiException("未实现方法");
    }
    /// <summary>
    /// 接收消息队立信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="subscriberName"></param>
    /// <param name="action"></param>
    public void ReceiveMq<T>(string subscriberName, Action<T> action)
    {
        throw new ApiException("未实现方法");
    }
}