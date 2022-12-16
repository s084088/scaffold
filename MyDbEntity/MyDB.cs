using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using MyDBEntity.Comm;
using MyDBEntity.Models;
using Util.Helper;

namespace MyDBEntity;

public class MyDB : DbContext
{
    private static string dBType;
    private static string sqlstr;
    private readonly bool _userLazyLoad;

    /// <summary>
    /// 构造函数(默认不使用懒加载)
    /// </summary>
    public MyDB() : this(false) { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userLazyLoad">是否启用懒加载</param>
    public MyDB(bool userLazyLoad) => _userLazyLoad = userLazyLoad;

    /// <summary>
    /// 保存所有更改并记录日志
    /// </summary>
    /// <param name="user">修改的用户,可以是任意字符串,也可为空,建议写userID或userName后台任务建议写system</param>
    /// <returns></returns>
    public int SaveChanges(string user)
    {
        //插入日志列表
        List<EfChangeLogModel> logs = ChangeTracker.WriteEFDataLog(user);
        //保存变动
        int count = base.SaveChanges();

        //保存成功后再记录日志
        logs.ForEach(l => LogHelper.WriteLog(l.Log, "logs/mytables/" + DateTime.Now.ToDefaultDateString(), l.TableName + ".table"));
        return count;
    }

    /// <summary>
    /// 保存所有更改并记录日志
    /// </summary>
    /// <param name="user">修改的用户,可以是任意字符串,也可为空,建议写userID或userName后台任务建议写system</param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(string user)
    {
        //插入日志列表
        List<EfChangeLogModel> logs = ChangeTracker.WriteEFDataLog(user);
        //保存变动
        int count = await base.SaveChangesAsync();

        //保存成功后再记录日志
        logs.ForEach(l => LogHelper.WriteLog(l.Log, "logs/mytables/" + DateTime.Now.ToDefaultDateString(), l.TableName + ".table"));
        return count;
    }

    /// <summary>
    /// 异步保存所有更改并记录日志(废弃)
    /// </summary>
    /// <returns></returns>
    [Obsolete("请使用带参数的SaveChangesAsync", true)]
    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await SaveChangesAsync("");

    /// <summary>
    /// 保存所有更改并记录日志(废弃)
    /// </summary>
    /// <returns></returns>
    [Obsolete("请使用带参数的SaveChanges", true)]
    public new int SaveChanges() => SaveChanges("");

    /// <summary>
    /// 数据库配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        CheckConfig();
        base.OnConfiguring(optionsBuilder);
        if (dBType == "SqlServer")
        {
            optionsBuilder
                .UseSqlServer(sqlstr)
                .ReplaceService<IQuerySqlGeneratorFactory, SqlServerNolockFactory>()
                ;
        }
        else if (dBType == "MySql")
        {
            optionsBuilder
                .UseMySql(sqlstr, new MariaDbServerVersion(new Version(10, 5, 18)))
                ;
        }
        else if (dBType == "Sqlite")
        {
            optionsBuilder
                .UseSqlite(sqlstr)
                ;
        }
        else throw new Exception("不支持的数据库类型");


        if (_userLazyLoad) optionsBuilder.UseLazyLoadingProxies();  //启用懒加载
        optionsBuilder.EnableSensitiveDataLogging(true).UseLoggerFactory(new LoggerFactory(new List<ILoggerProvider> { new EFLoggerProvider() })); //增加日志工厂
    }

    /// <summary>
    /// 模型建立
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        MethodInfo setTable = GetType().GetMethod("SetTable");
        foreach (Type tableType in Assembly.Load("MyDbEntity").GetTypes()
            .Where(l => l.BaseType == typeof(DbBase) ||
                        l.BaseType?.BaseType == typeof(DbBase) ||
                        l.BaseType?.BaseType?.BaseType == typeof(DbBase))
            .Where(l => l != typeof(FileBase))
            .ToArray())
            setTable.MakeGenericMethod(tableType).Invoke(this, new object[] { modelBuilder });

        modelBuilder.AddSeed();
    }

    public override IModel Model => base.Model;

    /// <summary>
    /// 没有数据库时自动创建
    /// </summary>
    public static void DbInit()
    {
        CheckConfig();
        new MyDB().Database.EnsureCreated();
    }

    /// <summary>
    /// 检查配置是否载入,没有载入则自动载入
    /// </summary>
    private static void CheckConfig()
    {
        if (sqlstr != null && dBType != null) return;
        sqlstr = ConfigurationHelper.GetValue("Environment:DBSetting:ConnectStr");
        dBType = ConfigurationHelper.GetValue("Environment:DBSetting:Type");
        Console.WriteLine($"{dBType} 连接字符串: {sqlstr}");
    }

    /// <summary>
    /// 映射对应表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="modelBuilder"></param>
    public static void SetTable<T>(ModelBuilder modelBuilder) where T : DbBase, new() => modelBuilder.Entity<T>();
}