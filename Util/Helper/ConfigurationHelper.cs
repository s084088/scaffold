using Microsoft.Extensions.Configuration;

namespace Util.Helper;
public static class ConfigurationHelper
{
    public static IConfiguration Configuration;
    static ConfigurationHelper()
    {
        // 这里读取环境变量
        var provider = new Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider();
        provider.Load();
        provider.TryGet("ASPNETCORE_ENVIRONMENT", out string environmentName);

        IConfigurationBuilder builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">不区分大小写</param>
    /// <returns></returns>
    public static string GetValue(string key)
    {
        return Configuration[key];
    }

    public static T GetValue<T>(string key)
    {
        return Configuration.GetValue<T>(key);
    }
}
