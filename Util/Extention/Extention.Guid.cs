using System.Security.Cryptography;

namespace Util;

public static partial class Extention
{
    public static readonly RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();

    /// <summary>
    /// 转为有序的GUID
    /// 注：长度为50字符
    /// </summary>
    /// <param name="guid">新的GUID</param>
    /// <returns></returns>
    public static string ToSequentialGuid(this Guid guid)
    {
        var timeStr = DateTime.Now.Ticks.ToString("x8");
        var newGuid = $"{timeStr.PadLeft(16, '0')}{guid:N}";

        return newGuid;
    }

    /// <summary>
    /// 获取序列Guid
    /// </summary>
    /// <returns></returns>
    public static Guid SequenceGuid()
    {
        byte[] randomBytes = new byte[8];
        randomNumberGenerator.GetBytes(randomBytes);                            //创建8个字节的强随机数
        byte[] timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);   //获取当前long时间戳 千万分之一秒
        if (BitConverter.IsLittleEndian) Array.Reverse(timestampBytes);         //根据CPU架构(是否低位在前),判断是否需要翻转字节
        byte[] guidBytes = new byte[16];                                        //创建16个字节的空间(待转GUID)
        Buffer.BlockCopy(timestampBytes, 0, guidBytes, 0, 8);                   //拷贝时间戳
        Buffer.BlockCopy(randomBytes, 0, guidBytes, 8, 8);                      //拷贝强随机数
        return new Guid(guidBytes);
    }
}