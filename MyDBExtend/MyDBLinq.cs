using System.Linq.Expressions;
using Util.Model;

namespace MyDBExtend
{
    public static class MyDBLinq
    {
        /// <summary>
        /// 根据指定条件检测数据库是否存在此数据,找不到的时候,直接抛API错误,基于FirstOrDefault
        /// </summary>
        /// <typeparam name="T">表类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="errMessage">报错信息</param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public static T ExistCheck<T>(this IQueryable<T> source, string errMessage = "获取数据失败") where T : class => source.FirstOrDefault() ?? throw new ApiException(errMessage);

        /// <summary>
        /// 根据指定条件检测数据库是否存在此数据,找不到的时候,直接抛API错误,基于FirstOrDefault
        /// </summary>
        /// <typeparam name="T">表类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="predicate">条件</param>
        /// <param name="errMessage">报错信息</param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public static T ExistCheck<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, string errMessage = "获取数据失败") where T : class => source.FirstOrDefault(predicate) ?? throw new ApiException(errMessage);

        /// <summary>
        /// 取表中非删除的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myDB"></param>
        /// <returns></returns>
        public static IQueryable<T> SetE<T>(this MyDB myDB) where T : DbBase => myDB.Set<T>().Where(l => l.DeleteFlag == 0);

        /// <summary>
        /// 取表中删除的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myDB"></param>
        /// <returns></returns>
        public static IQueryable<T> SetD<T>(this MyDB myDB) where T : DbBase => myDB.Set<T>().Where(l => l.DeleteFlag == 1);

        /// <summary>
        /// 无跟踪查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="query"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<R> SelectN<T, R>(this IQueryable<T> query, Expression<Func<T, R>> predicate) where T : class => query.AsNoTracking().Select(predicate);

        /// <summary>
        /// IQueryable方式 应用筛选先决条件判断数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate) => condition ? query.Where(predicate) : query;

        /// <summary>
        /// 尝试分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源IQueryable</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public static IQueryable<T> Pager<T>(this IQueryable<T> source, Pagination pagination)
        {
            if (pagination == null) return source;
            pagination.Total = source.Count();
            return source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
        }

        /// <summary>
        /// 取最大值,如果为空则为默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static R MaxOrDefault<T, R>(this IQueryable<T> source, Expression<Func<T, R?>> selector, R nullValue = default) where R : struct => source.Max(selector) ?? nullValue;

        /// <summary>
        /// 取最小值,如果为空则为默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static R MinOrDefault<T, R>(this IQueryable<T> source, Expression<Func<T, R?>> selector, R nullValue = default) where R : struct => source.Min(selector) ?? nullValue;
    }
}