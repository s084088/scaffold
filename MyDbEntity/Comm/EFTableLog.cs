using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using MyDBEntity.Models;

namespace MyDBEntity.Comm;

internal static class EFTableLog
{
    /// <summary>
    /// EF变更日志
    /// </summary>
    internal static List<EfChangeLogModel> WriteEFDataLog(this ChangeTracker changeTracker, string user)
    {
        //获取到EF变更条目
        List<EfChangeLogModel> logs = new();
        var list = changeTracker.Entries();
        foreach (var item in list)
        {
            //对应的表名
            string tableName = "";
            Type type = item.Entity.GetType();
            Type patientMngAttrType = typeof(TableAttribute);
            if (type.IsDefined(patientMngAttrType, true) && type.GetCustomAttributes(patientMngAttrType, true).FirstOrDefault() is TableAttribute attribute) tableName = attribute.Name;
            if (string.IsNullOrEmpty(tableName)) tableName = type.Name;

            switch (item.State)
            {
                case EntityState.Detached: break;
                case EntityState.Unchanged: break;
                case EntityState.Deleted: WriteEFDeleteLog(item, tableName.ToLower(), user, logs); break;
                case EntityState.Modified: WriteEFUpdateLog(item, tableName.ToLower(), user, logs); break;
                case EntityState.Added: WriteEFCreateLog(item, tableName.ToLower(), user, logs); break;
            }
        }
        return logs;
    }

    /// <summary>
    /// 记录EF创建操作日志
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="tableName"></param>
    /// <param name="user"></param>
    private static void WriteEFCreateLog(EntityEntry entry, string tableName, string user, List<EfChangeLogModel> logs)
    {
        StringBuilder sb = new();
        sb.Append($"user:{user} \t create");
        foreach (IProperty prop in entry.CurrentValues.Properties)
        {
            PropertyEntry entity = entry.Property(prop.Name);
            sb.Append($" \t {prop.Name}:{entity.CurrentValue}");
        }
        logs.Add(new EfChangeLogModel { Log = sb.ToString(), TableName = tableName });
    }

    /// <summary>
    /// 记录EF修改操作日志
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="tableName"></param>
    private static void WriteEFUpdateLog(EntityEntry entry, string tableName, string user, List<EfChangeLogModel> logs)
    {
        StringBuilder sb = new();
        PropertyEntry entity = entry.Property(nameof(DbBase.Id));
        sb.Append($"user:{ user } \t update \t ID:{entity.OriginalValue}");
        foreach (IProperty prop in entry.CurrentValues.Properties.Where(i => entry.Property(i.Name).IsModified))
        {
            entity = entry.Property(prop.Name);
            sb.Append($" \t {prop.Name}: {entity.OriginalValue} => {entity.CurrentValue}");
        }
        logs.Add(new EfChangeLogModel { Log = sb.ToString(), TableName = tableName });
        if (entry.Entity is DbBase dbBase) dbBase.UpdateTime = DateTime.Now;
    }

    /// <summary>
    /// 记录EF删除操作日志
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="tableName"></param>
    /// <param name="user"></param>
    private static void WriteEFDeleteLog(EntityEntry entry, string tableName, string user, List<EfChangeLogModel> logs)
    {
        StringBuilder sb = new();
        sb.Append($"user:{ user } \t delete");
        foreach (IProperty prop in entry.CurrentValues.Properties)
        {
            PropertyEntry entity = entry.Property(prop.Name);
            sb.Append($" \t {prop.Name}:{entity.OriginalValue}");
        }
        logs.Add(new EfChangeLogModel { Log = sb.ToString(), TableName = tableName });
    }
}