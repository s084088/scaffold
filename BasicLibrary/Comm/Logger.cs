namespace BasicLibrary.Comm;

public static class Logger
{
    /// <summary>
    /// 系统管理员日志
    /// </summary>
    /// <param name="str"></param>
    public static void AdminLog(string str)
    {
        LogHelper.WriteLog(str, "logs/AdminLog", DateTime.Now.ToDefaultDateString() + ".log");
    }
}
