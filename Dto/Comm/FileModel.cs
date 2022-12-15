namespace Dto.Comm;

/// <summary>
/// 文件模型
/// </summary>
/// <param name="Bucket">桶名</param>
/// <param name="Path">路径</param>
/// <param name="FileName">文件名</param>
/// <param name="Extention">扩展名</param>
/// <param name="MD5">校验MD5</param>
public record OssFileModel(string Bucket, string Path, string FileName, string Extention, string MD5) : IdDto;