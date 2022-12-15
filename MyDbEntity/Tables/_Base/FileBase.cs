namespace MyDBEntity;

/// <summary>
/// 文件表基表
/// </summary>
public abstract class FileBase : DbBase
{
    /// <summary>
    /// 区域
    /// </summary>
    [MaxLength(31)]
    public string OssRegion { get; set; }

    /// <summary>
    /// Bucket
    /// </summary>
    [MaxLength(31)]
    public string OssBucket { get; set; }

    /// <summary>
    /// OSS存储路径
    /// </summary>
    [MaxLength(63)]
    public string OssPath { get; set; }

    /// <summary>
    /// OSS存储文件名
    /// </summary>
    [MaxLength(63)]
    public string OssName { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    [MaxLength(15)]
    public string OssExtention { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// 文件MD5
    /// </summary>
    [MaxLength(63)]
    public string FileMD5 { get; set; }
}