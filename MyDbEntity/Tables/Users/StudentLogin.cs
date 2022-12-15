namespace MyDBEntity.Tables.Users;

[Table("Users_Student_Login")]
public class StudentLogin : DbBase
{
    /// <summary>
    /// 登录方式  1,Web
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 客户端局域网IP
    /// </summary>
    [MaxLength(31)]
    public string LANIP { get; set; }

    /// <summary>
    /// 客户端万维网IP
    /// </summary>
    [MaxLength(31)]
    public string WANIP { get; set; }

    /// <summary>
    /// 客户端设备UUID
    /// </summary>
    [MaxLength(63)]
    public string UUID { get; set; }

    /// <summary>
    /// 主机名
    /// </summary>
    [MaxLength(63)]
    public string HostName { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    [Column(TypeName = "decimal(10,5)")]
    public decimal Lng { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    [Column(TypeName = "decimal(10,5)")]
    public decimal Lat { get; set; }

    /// <summary>
    /// 登录位置
    /// </summary>
    [MaxLength(127)]
    public string Location { get; set; }

    /// <summary>
    /// 系统版本号
    /// </summary>
    [MaxLength(31)]
    public string SystemVersion { get; set; }

    /// <summary>
    /// 应用版本号
    /// </summary>
    [MaxLength(31)]
    public string AppVersion { get; set; }

    /// <summary>
    /// 内部版本号
    /// </summary>
    [MaxLength(31)]
    public string AppBuild { get; set; }

    /// <summary>
    /// 登出时间
    /// </summary>
    public DateTime? LogoutTime { get; set; }

    /// <summary>
    /// 对应学生
    /// </summary>
    public virtual Student Student { get; set; }
    public string StudentID { get; set; }
}