namespace Util;

/// <summary>
/// 缓存帮助类
/// </summary>
public class CacheHelper
{
    /// <summary>
    /// 缓存类型,1系统缓存,2redis缓存
    /// </summary>
    public static int Type { get; } = 1;

    /// <summary>
    /// 默认redis库
    /// </summary>
    public static int RedisIndex { get; set; } = 0;

    /// <summary>
    /// redis配置
    /// </summary>
    public static string RedisConfig { get; set; }

    /// <summary>
    /// 静态构造函数，初始化缓存类型
    /// </summary>
    static CacheHelper()
    {
        SystemCache = new SystemCache();
    }
    /// <summary>
    ///初始化
    /// </summary>
    public static void Init()
    {

        if (!RedisConfig.IsNullOrEmpty())
        {
            try
            {
                RedisCache = new RedisCache(RedisConfig, RedisIndex);

                Console.WriteLine($"默认Redis库: {RedisConfig} , Index: {RedisIndex}");
            }
            catch
            {
            }
        }

        switch (Type)
        {
            case 1: Cache = SystemCache; break;
            case 2: Cache = RedisCache; break;
            default: throw new Exception("请指定缓存类型！");
        }
    }
    /// <summary>
    /// 默认缓存
    /// </summary>
    public static ICache Cache { get; set; }

    /// <summary>
    /// 系统缓存
    /// </summary>
    public static ICache SystemCache { get; set; }

    /// <summary>
    /// Redis缓存
    /// </summary>
    public static ICache RedisCache { get; set; }
}