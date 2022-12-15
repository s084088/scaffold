using NodaTime;
using System.Globalization;

namespace Util;

public static partial class Extention
{
    ///   <summary>
    ///  获取某一日期是该年中的第几周
    ///   </summary>
    ///   <param name="dateTime"> 日期 </param>
    ///   <returns> 该日期在该年中的周数 </returns>
    public static int GetWeekOfYear(this DateTime dateTime) => new GregorianCalendar().GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

    /// <summary>
    /// 获取Js格式的timestamp
    /// </summary>
    /// <param name="dateTime">日期</param>
    /// <returns></returns>
    public static long ToJsTimestamp(this DateTime dateTime) => (dateTime.Ticks - TimeZoneInfo.ConvertTimeFromUtc(new DateTime().Default(), TimeZoneInfo.Local).Ticks) / 10000;

    /// <summary>
    /// 获取js中的getTime()
    /// </summary>
    /// <param name="dt">日期</param>
    /// <returns></returns>
    public static long JsGetTime(this DateTime dt) => (long)((dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds + 0.5);

    /// <summary>
    /// 返回默认时间1970-01-01
    /// </summary>
    /// <returns></returns>
    public static DateTime Default(this DateTime _) => DateTime.Parse("1970-01-01");

    /// <summary>
    /// 转为标准时间（北京时间，解决Linux时区问题）
    /// </summary>
    /// <returns></returns>
    public static DateTime ToCstTime(this DateTime _) => SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb["Asia/Shanghai"]).ToDateTimeUnspecified();

    /// <summary>
    /// 转化为默认时间字符串
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToDefaultString(this DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss");

    /// <summary>
    /// 转化为默认日期字符串
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToDefaultDateString(this DateTime dt) => dt.ToString("yyyy-MM-dd");

    /// <summary>
    /// 取本周第一天
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime GetWeekFirstDay(this DateTime dt)
    {
        dt = dt.Date;
        int weeknow = Convert.ToInt32(dt.DayOfWeek);
        weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
        return dt.AddDays(-weeknow);
    }

    /// <summary>
    /// 去除时间戳,小数点后面的部分
    /// </summary>
    /// <param name="ts"></param>
    /// <returns></returns>
    public static TimeSpan RemoveMilliseconds(this TimeSpan ts) => TimeSpan.FromSeconds((int)ts.TotalSeconds);

    /// <summary>
    /// 是否比当前时间更大
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static bool ThenNow(this DateTime dateTime) => dateTime > DateTime.Now;
}