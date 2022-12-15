using Microsoft.EntityFrameworkCore;
using MyDBEntity.Tables.Users;

namespace MyDBEntity.Comm;

/// <summary>
/// 种子数据扩展类
/// </summary>
internal static class Seed
{
    /// <summary>
    /// 添加种子数据
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ModelBuilder AddSeed(this ModelBuilder builder)
    {
        Student student1 = new (){ Name = "宋雨雨", Account = "syyy", Password = "syyy"};
        builder.Entity<Student>().HasData(student1);


        return builder;
    }
}