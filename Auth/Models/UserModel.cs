using System;

namespace Auth.Models;

/// <summary>
/// 用户模型
/// </summary>
public class UserModel
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 秘钥,Key
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 登陆时间
    /// </summary>
    public DateTime LoginTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime LastTime { get; set; }
}