using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Util;

/// <summary>
/// Redis缓存
/// </summary>
public class RedisCache : ICache
{
    /// <summary>
    /// 默认构造函数
    /// 注：使用默认配置，即localhost:6379,无密码
    /// </summary>
    public RedisCache()
    {
        DatabaseIndex = 0;
        string config = "localhost:6379";
        RedisConnection = ConnectionMultiplexer.Connect(config);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="config">配置字符串</param>
    /// <param name="databaseIndex">数据库索引</param>
    public RedisCache(string config, int databaseIndex = 0)
    {
        DatabaseIndex = databaseIndex;
        ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(config);
        configurationOptions.SyncTimeout = 10000;
        configurationOptions.ConnectTimeout = 10000;
        RedisConnection = ConnectionMultiplexer.Connect(configurationOptions);
    }
    private ConnectionMultiplexer RedisConnection { get; set; }
    private IDatabase Db => RedisConnection.GetDatabase(DatabaseIndex);
    private int DatabaseIndex { get; }

    public bool ContainsKey(string key)
    {
        return Db.KeyExists(key);
    }

    public object GetCache(string key)
    {
        var redisValue = Db.StringGet(key);
        if (!redisValue.HasValue)
            return null;
        ValueInfoEntry valueEntry = redisValue.ToString().ToObject<ValueInfoEntry>();
        object value = valueEntry.TypeName == typeof(string).FullName
            ? valueEntry.Value
            : valueEntry.Value.ToObject(Type.GetType(valueEntry.TypeName));
        if (valueEntry.ExpireTime != null && valueEntry.ExpireType == ExpireType.Relative)
            SetKeyExpire(key, valueEntry.ExpireTime.Value);

        return value;
    }

    public T GetCache<T>(string key) where T : class
    {
        object value = GetCache(key);
        if (value == null) return null;
        return (JToken.Parse(value.ToJson())).ToObject<T>();
    }

    public void SetKeyExpire(string key, TimeSpan expire)
    {
        Db.KeyExpire(key, expire);
    }

    public void RemoveCache(string key)
    {
        Db.KeyDelete(key);
    }

    public void SetCache(string key, object value)
    {
        SetCache1(key, value, null, null);
    }

    public void SetCache(string key, object value, TimeSpan timeout)
    {
        SetCache1(key, value, timeout, ExpireType.Absolute);
    }

    public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType)
    {
        SetCache1(key, value, timeout, expireType);
    }

    private void SetCache1(string key, object value, TimeSpan? timeout, ExpireType? expireType)
    {
        string jsonStr = value is string ? value as string : value.ToJson();
        ValueInfoEntry entry = new()
        {
            Value = jsonStr,
            TypeName = value.GetType().FullName,
            ExpireTime = timeout,
            ExpireType = expireType
        };

        string theValue = entry.ToJson();
        if (timeout == null)
            Db.StringSet(key, theValue);
        else
            Db.StringSet(key, theValue, timeout);
    }
    /// <summary>
    /// 消息添加到队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="subscriberName"></param>
    /// <param name="data"></param>
    public void PutMq<T>(string subscriberName, T data)
    {
        ISubscriber sub = RedisConnection.GetSubscriber();
        sub.Publish(subscriberName, data.ToJson());
    }
    /// <summary>
    /// 接收消息队立信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="subscriberName"></param>
    /// <param name="action"></param>
    public void ReceiveMq<T>(string subscriberName, Action<T> action)
    {
        ISubscriber sub = RedisConnection.GetSubscriber();

        //订阅名为 messages 的通道

        sub.Subscribe(subscriberName, (channel, message) =>
        {

            //输出收到的消息
            var entity = JsonConvert.DeserializeObject<T>(message);
            action.Invoke(entity);
        });
    }
}