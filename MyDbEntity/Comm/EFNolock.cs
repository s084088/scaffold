using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace MyDBEntity.Comm;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
internal class SqlServerNoLock : SqlServerQuerySqlGenerator
{
    public SqlServerNoLock(QuerySqlGeneratorDependencies dependencies) : base(dependencies)
    {
    }

    protected override Expression VisitTable(TableExpression tableExpression)
    {
        var result = base.VisitTable(tableExpression);
        Sql.Append(" WITH (NOLOCK)");
        return result;
    }
}

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
internal class SqlServerNolockFactory : SqlServerQuerySqlGeneratorFactory
{
    private readonly QuerySqlGeneratorDependencies _dependencies;

    public SqlServerNolockFactory(QuerySqlGeneratorDependencies dependencies) : base(dependencies)
    {
        _dependencies = dependencies;
    }

    public override QuerySqlGenerator Create()
    {
        return new SqlServerNoLock(_dependencies);
    }
}