using System.ComponentModel.DataAnnotations;

namespace Dto.Comm;

/// <summary>
/// 测试模型
/// </summary>
/// <param name="String">字符串</param>
/// <param name="Null">空对象</param>
/// <param name="Decimal">数字</param>
/// <param name="Bool">布尔值</param>
public record TestModel([Required(ErrorMessage = "String字符串必填项")] string String = "连接OK", object Null = null, decimal Decimal = -123.456m, bool Bool = false)
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime DateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 时间戳
    /// </summary>
    public TimeSpan TimeSpan { get; set; } = new TimeSpan(700, 13, 13, 13, 988);

    /// <summary>
    /// 数组
    /// </summary>
    public List<TestModel> Items { get; set; } = new List<TestModel>();
}