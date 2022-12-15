﻿using System.Xml.Serialization;

namespace Util;

/// <summary>
/// XML文档操作帮助类
/// </summary>
public class XmlHelper
{
    /// <summary>
    /// 序列化为XML字符串
    /// </summary>
    /// <param name="obj">对象</param>
    /// <returns></returns>
    public static string Serialize(object obj)
    {
        Type type = obj.GetType();
        MemoryStream Stream = new();
        XmlSerializer xml = new(type);
        try
        {
            //序列化对象
            xml.Serialize(Stream, obj);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        Stream.Position = 0;
        StreamReader sr = new(Stream);
        string str = sr.ReadToEnd();

        sr.Dispose();
        Stream.Dispose();

        return str;
    }
}