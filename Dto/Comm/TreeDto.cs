namespace Dto.Comm;

/// <summary>
/// 树形结构DTO
/// </summary>
public record TreeDto : IdNameDto
{
    /// <summary>
    /// 子集
    /// </summary>
    public IEnumerable<TreeDto> Children { get; set; }
}