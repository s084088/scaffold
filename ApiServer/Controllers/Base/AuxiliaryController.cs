namespace ApiServer.Controllers.Base;

[Route("Api/Base/Auxiliary")]
[ApiExplorerSettings(GroupName = "Base")]
[CustomAuthorize(EnumCustomAuthorize.None)]
public class AuxiliaryController : BaseController
{
    /// <summary>
    /// 获取Oss的Token
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetOssToken")]
    public WebApiPackage GetOssToken()
    {
        var ret = new { ossToken = OssHelper.GetOssToken() };
        return OK(ret);
    }
}