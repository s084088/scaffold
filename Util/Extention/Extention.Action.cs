using System.Diagnostics;

namespace Util;

public static partial class Extention
{
    /// <summary>
    /// 执行Action委托并获取时间
    /// </summary>
    /// <param name="action">委托</param>
    /// <returns></returns>
    public static TimeSpan GetActionTime(this Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    /// <summary>
    /// 执行Action委托并获取时间
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action">委托</param>
    /// <param name="para">参数</param>
    /// <returns></returns>
    public static TimeSpan GetActionTime<T>(this Action<T> action, T para)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action(para);
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}