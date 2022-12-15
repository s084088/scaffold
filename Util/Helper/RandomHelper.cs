namespace Util;

/// <summary>
/// Random随机数帮助类
/// </summary>
public static class RandomHelper
{
    private static Random Random { get; } = new Random();

    /// <summary>
    /// 下一个随机数 (包含最小值,不包含最大值)
    /// </summary>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns></returns>
    public static int Next(int minValue, int maxValue) => Random.Next(minValue, maxValue);

    /// <summary>
    /// 下一个随机值
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="source">值的集合</param>
    /// <returns></returns>
    public static T Next<T>(IEnumerable<T> source) => source.ToList()[Next(0, source.Count())];

    /// <summary>
    /// 获取随机概率  一定概率返回true
    /// </summary>
    /// <param name="chance">概率 0-1</param>
    /// <returns></returns>
    public static bool Chance(double chance) => chance > Random.NextDouble();

    /// <summary>
    /// 取list中随机几个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="randomNum"></param>
    /// <returns></returns>
    public static List<T> RandomNext<T>(IEnumerable<T> source, int randomNum)
    {
        List<T> sourceList = source.ToList();
        List<T> result = new();

        if (sourceList.Count > randomNum)
        {
            for (int i = 0; i < randomNum; i++)
            {
                result.Add(sourceList[Next(0, source.Count())]);
            }
        }
        return result.Distinct().ToList();
    }
}