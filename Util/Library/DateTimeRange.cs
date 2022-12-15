namespace Util.Library;

/// <summary>
/// 时间范围
/// </summary>
public struct DateTimeRange
{
    /// <summary>
    /// 起始
    /// </summary>
    public DateTime Start { get; init; }

    /// <summary>
    /// 结束
    /// </summary>
    public DateTime End { get; init; }

    /// <summary>
    /// 标准构造函数
    /// </summary>
    /// <param name="dt1">时间1</param>
    /// <param name="dt2">时间2</param>
    public DateTimeRange(DateTime dt1, DateTime dt2)
    {
        if (dt1 == dt2) throw new ArgumentException("两个时间不能相同");

        //动态调整结构体
        //确保Start一定小于End
        if (dt1 > dt2)
        {
            Start = dt2;
            End = dt1;
        }
        else
        {
            Start = dt1;
            End = dt2;
        }
    }

    /// <summary>
    /// 默认构造函数,取最大时间范围
    /// </summary>
    public DateTimeRange()
    {
        Start = DateTime.MinValue;
        End = DateTime.MaxValue;
    }
}

/// <summary>
/// 时间范围扩展方法
/// </summary>
public static class DateTimeRangeExtensions
{
    /// <summary>
    /// 判断当前时间是否在某时间范围
    /// </summary>
    /// <param name="data">时间</param>
    /// <param name="range">范围</param>
    /// <param name="includeMin">是否包含最小值,默认包含</param>
    /// <param name="includeMax">是否包含最大值,默认不包含</param>
    /// <returns>
    /// 返回差异,在范围内则为TimeSpan.Zero,否则返回与时间范围的差异
    /// 若低于最低时间:返回与最低时间的差异,TimeSpan为负数
    /// 若高于最高时间:返回与最高时间的差异,TimeSpan为正数
    /// </returns>
    public static TimeSpan InRange(this DateTime data, DateTimeRange range, bool includeMin = true, bool includeMax = false) => data.InRange(range.Start, range.End, includeMin, includeMax);

    /// <summary>
    /// 判断当前时间范围是否包含某时间
    /// </summary>
    /// <param name="data">时间</param>
    /// <param name="range">范围</param>
    /// <param name="includeMin">是否包含最小值,默认包含</param>
    /// <param name="includeMax">是否包含最大值,默认不包含</param>
    /// <returns>
    /// 返回差异,在范围内则为TimeSpan.Zero,否则返回与时间范围的差异
    /// 若低于最低时间:返回与最低时间的差异,TimeSpan为负数
    /// 若高于最高时间:返回与最高时间的差异,TimeSpan为正数
    /// </returns>
    public static TimeSpan HasTime(this DateTimeRange range, DateTime data, bool includeMin = true, bool includeMax = false) => data.InRange(range.Start, range.End, includeMin, includeMax);

    /// <summary>
    /// 判断当前时间是否在某时间范围
    /// </summary>
    /// <param name="data">时间</param>
    /// <param name="minData">范围最小值</param>
    /// <param name="maxData">范围最大值</param>
    /// <param name="includeMin">是否包含最小值,默认包含</param>
    /// <param name="includeMax">是否包含最大值,默认不包含</param>
    /// <returns>
    /// 返回差异,在范围内则为TimeSpan.Zero,否则返回与时间范围的差异
    /// 若低于最低时间:返回与最低时间的差异,TimeSpan为负数
    /// 若高于最高时间:返回与最高时间的差异,TimeSpan为正数
    /// </returns>
    public static TimeSpan InRange(this DateTime data, DateTime minData, DateTime maxData, bool includeMin = true, bool includeMax = false)
    {
        if (includeMin) minData = minData.AddTicks(1);
        if (includeMax) maxData = maxData.AddTicks(-1);

        if (data < minData) return data - minData;
        else if (data > maxData) return data - maxData;
        else return TimeSpan.Zero;
    }

    /// <summary>
    /// 比较两个时间范围,是否有重叠
    /// </summary>
    /// <param name="range1">时间范围1</param>
    /// <param name="range2">时间范围2</param>
    /// <returns></returns>
    public static bool Compare(this DateTimeRange range1, DateTimeRange range2)
    {
        if (range1.End <= range2.Start) return false;
        if (range1.Start >= range2.End) return false;
        return true;
    }

    /// <summary>
    /// 比较两个时间范围,是否有重叠,并返回详细信息
    /// </summary>
    /// <param name="range1">时间范围1</param>
    /// <param name="range2">时间范围2</param>
    /// <returns>
    /// 1,不重叠,目标范围小于当前范围
    /// 2,有重叠,目标范围小于当前范围
    /// 3,有重叠,目标范围包含当前范围
    /// 4,有重叠,目标范围等于当前范围
    /// 5,有重叠,当前范围包含目标范围
    /// 6,有重叠,目标范围大于当前范围
    /// 7,不重叠,目标范围大于当前范围
    /// </returns>
    public static int CompareDetail(this DateTimeRange range1, DateTimeRange range2)
    {
        if (range2.Start < range1.Start)
        {
            if (range2.End < range1.Start) return 1;
            else if (range2.End == range1.Start) return 1;
            else
            {
                if (range2.End < range1.End) return 2;
                else if (range2.End == range1.End) return 3;
                else return 3;
            }
        }
        else if (range2.Start == range1.Start)
        {
            if (range2.End < range1.End) return 5;
            else if (range2.End == range1.End) return 4;
            else return 3;
        }
        else if (range2.Start < range1.End)
        {
            if (range2.End > range1.End) return 5;
            else if (range2.End == range1.End) return 5;
            else return 6;
        }
        else return 7;
    }
}