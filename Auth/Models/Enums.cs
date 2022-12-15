namespace Auth.Models;

/// <summary>
/// 验证类型
/// </summary>
public enum ValidationResult
{
    /// <summary>
    /// 成功
    /// </summary>
    Success,

    /// <summary>
    /// Token过期
    /// </summary>
    Expired,

    /// <summary>
    /// Token无效
    /// </summary>
    Invalid,
}