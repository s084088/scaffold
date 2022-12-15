namespace MyDBEntity.Models;

/// <summary>
/// 日志模型
/// </summary>
internal class EfChangeLogModel
{
    /// <summary>
    /// 表名
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 日志信息
    /// </summary>
    public string Log { get; set; }
}