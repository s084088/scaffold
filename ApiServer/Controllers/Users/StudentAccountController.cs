using BasicLibrary.Users;
using Dto.Auth;

namespace ApiServer.Controllers.Users;

/// <summary>
/// 学生账号接口
/// </summary>
[Route("Api/Users/StudentAccount")]
[ApiExplorerSettings(GroupName = "Users")]
public class StudentAccountController : BaseController
{
    private StudentAccountServer _server = new();

    /// <summary>
    /// 登陆
    /// </summary>
    /// <param name="inDto"></param>
    /// <returns></returns>
    [HttpPost("LoginByAccount")]
    [CustomAuthorize(EnumCustomAuthorize.None)]
    public WebApiPackage<StudentLoginOutDto> LoginByAccount(StudentLoginInDto inDto)
    {
        string token = _server.Login(inDto.Account, inDto.Password);

        return Data(new StudentLoginOutDto(token));
    }
}