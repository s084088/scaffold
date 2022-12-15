using Microsoft.Extensions.Logging;

namespace MyDBEntity.Comm;

internal class EFLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new MyEFLogger(categoryName);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

internal class MyEFLogger : ILogger
{
    private readonly string categoryName;
    private const string line = "\r\n---------------------------------------------------------------------------------------------------------------------------";
    private const string commandName = "Microsoft.EntityFrameworkCore.Database.Command";

    public MyEFLogger(string categoryName) => this.categoryName = categoryName;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (logLevel == LogLevel.Information && categoryName == commandName)
        {
            var logContent = Environment.NewLine + formatter(state, exception) + line;
            LogHelper.WriteLog(logContent, "logs/myef", DateTime.Now.ToDefaultDateString() + ".sql");
            int count = Convert.ToInt32(((IReadOnlyList<KeyValuePair<string, object>>)state).FirstOrDefault(l => l.Key == "elapsed").Value?.ToString()?.Replace(",", "") ?? "0");
            if (count > 100)
            {
                if (count < 200) LogHelper.WriteLog(logContent, "logs/myef", DateTime.Now.ToDefaultDateString() + "_longTime100_200.sql");
                else if (count < 500) LogHelper.WriteLog(logContent, "logs/myef", DateTime.Now.ToDefaultDateString() + "_longTime200_500.sql");
                else if (count < 1000) LogHelper.WriteLog(logContent, "logs/myef", DateTime.Now.ToDefaultDateString() + "_longTime500_1000.sql");
                else LogHelper.WriteLog(logContent, "logs/myef", DateTime.Now.ToDefaultDateString() + "_longTime1000_.sql");
            }
        }
        else if (logLevel == LogLevel.Error)
        {
            var logContent = Environment.NewLine + formatter(state, exception) + line;
            LogHelper.WriteLog(logContent, "logs/myef", DateTime.Now.ToDefaultDateString() + "_error.sql");
        }
    }

    public IDisposable BeginScope<TState>(TState state) => null;
}