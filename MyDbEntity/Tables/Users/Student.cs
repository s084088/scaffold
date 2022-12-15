namespace MyDBEntity.Tables.Users;

[Table("Users_Student")]
public class Student : DbBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(31)]
    public string Name { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [MaxLength(31)]
    public string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [MaxLength(63)]
    public string Password { get; set; }
}