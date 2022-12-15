using Auth.Models;

namespace ApiServer.Comm;

/// <summary>
/// 单个请求生命周期的临时数据
/// </summary>

public class RequestTemporaryData
{
    /// <summary>
    /// 对应用户
    /// </summary>
    public UserModel User { get; set; }

    /// <summary>
    /// 单词随机ID
    /// </summary>
    public string Id { get; init; } = GuidHelper.GenerateKey();

    /// <summary>
    /// 远程IP
    /// </summary>
    public string RemoteAddress { get; set; }

    /// <summary>
    /// 访问接口
    /// </summary>
    public string UrlPath { get; set; }

    /// <summary>
    /// 当前时间
    /// </summary>
    public DateTime CreateTime { get; init; } = DateTime.Now;

    /// <summary>
    /// 当前日期
    /// </summary>
    public string Data { get; set; } = DateTime.Now.ToDefaultDateString();

    /// <summary>
    /// 当次请求的Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 设备ID
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// 获取日志文本(远程地址,访问路径,token,设备id,请求id)(前后都有制表符)
    /// </summary>
    public string LogPrefix => $" \t IP:{RemoteAddress} \t PATH:{UrlPath} \t Guid:{Id} \t Token:{Token} \t DeviceID:{DeviceId} \t ";
}