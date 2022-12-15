namespace Dto.Comm;

/// <summary>
/// 包含ID的模型
/// </summary>
public record IdDto
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; }
}

/// <summary>
/// 包含ID和Name的模型
/// </summary>
public record IdNameDto : IdDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// 包含Name的模型
/// </summary>
public record NameDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}