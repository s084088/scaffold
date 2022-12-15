namespace Dto.Auth;

/// <summary>
/// 学生登陆入参
/// </summary>
/// <param name="Account">账号</param>
/// <param name="Password">密码</param>
public record StudentLoginInDto(string Account, string Password);

/// <summary>
/// 学生登陆出参
/// </summary>
/// <param name="Token"></param>
public record StudentLoginOutDto(string Token);