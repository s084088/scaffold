using Newtonsoft.Json;

using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Util;

public static partial class Extention
{
    /// <summary>
    /// 转为字节数组
    /// </summary>
    /// <param name="base64Str">base64字符串</param>
    /// <returns></returns>
    public static byte[] ToBytes_FromBase64Str(this string base64Str)
    {
        return Convert.FromBase64String(base64Str);
    }

    /// <summary>
    /// 转换为MD5加密后的字符串（默认加密为32位）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToMD5String(this string str, string key = "lyxaxssyy")
    {
        string str1 = str + key;
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(str1);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }
        md5.Dispose();

        return sb.ToString();
    }

    /// <summary>
    /// Base64加密
    /// 注:默认采用UTF8编码
    /// </summary>
    /// <param name="source">待加密的明文</param>
    /// <returns>加密后的字符串</returns>
    public static string Base64Encode(this string source)
    {
        return Base64Encode(source, Encoding.UTF8);
    }

    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="source">待加密的明文</param>
    /// <param name="encoding">加密采用的编码方式</param>
    /// <returns></returns>
    public static string Base64Encode(this string source, Encoding encoding)
    {
        string encode = string.Empty;
        byte[] bytes = encoding.GetBytes(source);
        try
        {
            encode = Convert.ToBase64String(bytes);
        }
        catch
        {
            encode = source;
        }
        return encode;
    }

    /// <summary>
    /// Base64解密
    /// 注:默认使用UTF8编码
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string Base64Decode(this string result)
    {
        return Base64Decode(result, Encoding.UTF8);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <param name="encoding">解密采用的编码方式，注意和加密时采用的方式一致</param>
    /// <returns>解密后的字符串</returns>
    public static string Base64Decode(this string result, Encoding encoding)
    {
        string decode = string.Empty;
        byte[] bytes = Convert.FromBase64String(result);
        try
        {
            decode = encoding.GetString(bytes);
        }
        catch
        {
            decode = result;
        }
        return decode;
    }

    /// <summary>
    /// 计算SHA1摘要
    /// 注：默认使用UTF8编码
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static byte[] ToSHA1Bytes(this string str)
    {
        return str.ToSHA1Bytes(Encoding.UTF8);
    }

    /// <summary>
    /// 计算SHA1摘要
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static byte[] ToSHA1Bytes(this string str, Encoding encoding)
    {
        SHA1 sha1 = SHA1.Create();
        byte[] inputBytes = encoding.GetBytes(str);
        byte[] outputBytes = sha1.ComputeHash(inputBytes);

        return outputBytes;
    }

    /// <summary>
    /// string转int
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        str = str?.Replace("\0", "");
        if (string.IsNullOrEmpty(str))
            return 0;
        return Convert.ToInt32(str);
    }

    /// <summary>
    /// 转换为日期格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string str)
    {
        return Convert.ToDateTime(str);
    }

    /// <summary>
    /// 将Json字符串反序列化为对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="jsonStr">Json字符串</param>
    /// <returns></returns>
    public static T ToObject<T>(this string jsonStr)
    {
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }

    /// <summary>
    /// 缩写字符串
    /// </summary>
    /// <param name="text">原字符串</param>
    /// <param name="length">最大长度</param>
    /// <param name="last">超过最大长度后的缩写</param>
    /// <returns></returns>
    public static string Abbreviation(this string text, int length, string last = "...")
    {
        if (text.Length > length) text = string.Concat(text.AsSpan(0, length), last);
        return text;
    }

    /// <summary>
    /// 将Json字符串转为DataTable
    /// </summary>
    /// <param name="jsonStr">Json字符串</param>
    /// <returns></returns>
    public static DataTable ToDataTable(this string jsonStr)
    {
        return jsonStr == null ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);
    }

    /// <summary>
    /// 转为流量字符串
    /// </summary>
    /// <param name="bytes">流量大小</param>
    /// <returns></returns>
    public static string ToTrafficString(this long bytes)
    {
        if (bytes > 0)
        {
            if (bytes < 1000) return bytes.ToString() + "B";
            if (bytes < 1000 * 1024) return (bytes / 1024D).ToString("#.##") + "KB";
            if (bytes < 1000 * 1024 * 1024) return (bytes / 1024D / 1024D).ToString("#.##") + "MB";
            if (bytes < 1000D * 1024 * 1024 * 1024) return (bytes / 1024D / 1024D / 1024D).ToString("#.##") + "GB";
            else return (bytes / 1024D / 1024D / 1024D / 1024D).ToString("#.##") + "TB";
        }
        else
        {
            if (bytes > -1000) return bytes.ToString() + "B";
            if (bytes > -1000 * 1024) return (bytes / 1024D).ToString("#.##") + "KB";
            if (bytes > -1000 * 1024 * 1024) return (bytes / 1024D / 1024D).ToString("#.##") + "MB";
            if (bytes > -1000D * 1024 * 1024 * 1024) return (bytes / 1024D / 1024D / 1024D).ToString("#.##") + "GB";
            else return (bytes / 1024D / 1024D / 1024D / 1024D).ToString("#.##") + "TB";
        }
    }

    /// <summary>
    /// 去除原字符串结尾处的所有替换字符串
    /// 如：原字符串"sdlfjdcdcd",替换字符串"cd" 返回"sdlfjd"
    /// </summary>
    /// <param name="strSrc">源字符串</param>
    /// <param name="strTrim">去除的字符串</param>
    /// <param name="isLoop">是否循环去除,默认为true</param>
    /// <returns></returns>
    public static string TrimEnd(this string strSrc, string strTrim, bool isLoop = true)
    {
        if (string.IsNullOrEmpty(strSrc) || string.IsNullOrEmpty(strTrim)) return strSrc;
        if (strSrc.EndsWith(strTrim))
        {
            string strDes = strSrc[..^strTrim.Length];
            return !isLoop ? strDes : strDes.TrimEnd(strTrim);
        }
        return strSrc;
    }

    /// <summary>
    /// 删除字符串内的空格和回车
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string RemoveAndEnter(this string str)
    {
        return str?.Replace("\r", "").Replace("\n", "").Replace(" ", "");
    }

    /// <summary>
    /// 返回该字符串是否手机号
    /// </summary>
    /// <param name="str">源字符串</param>
    /// <returns></returns>
    public static bool IsHandset(this string str)
    {
        return Regex.IsMatch(str, @"^1[3456789]\d{9}$");
    }
}