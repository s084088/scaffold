using Dto.Comm;

namespace MyDBExtend.Tables;
public static class FileExtend
{
    /// <summary>
    /// 获取文件对象
    /// </summary>
    /// <returns></returns>
    public static OssFileModel GetFile(this FileBase f) => f == null ? null : new OssFileModel(f.OssBucket, f.OssPath, f.OssName, f.OssExtention, f.FileMD5) { Id = f.Id };
}
