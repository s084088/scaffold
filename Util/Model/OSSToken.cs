namespace Util;

public class OSSTokenEntity
{
    public string AccessKeyId { get; set; }
    public string AccessKeySecret { get; set; }
    public string SecurityToken { get; set; }
    public DateTime Expiration { get; set; }
}