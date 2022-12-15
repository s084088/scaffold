using Newtonsoft.Json.Linq;
using Util.Model;

namespace Util;

public static partial class Extention
{
    /// <summary>
    /// 获取json值,若取不到,返回空或者指定错误
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="field"></param>
    /// <param name="need"></param>
    /// <returns></returns>
    public static T GetApiValue<T>(this JObject json, string field, bool need = false)
    {
        try
        {
            if (json.Property(field, StringComparison.OrdinalIgnoreCase) == null)
            {
                if (need) throw new Exception();
                else return default;
            }
            return json.GetValue(field, StringComparison.OrdinalIgnoreCase).Value<T>();
        }
        catch
        {
            if (need) throw new ApiException("参数有误 请填写正确的" + field);
            return default;
        }
    }

    /// <summary>
    /// 获取json值,若取不到,返回指定值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="field"></param>
    /// <param name="defult"></param>
    /// <returns></returns>
    public static T GetApiValue<T>(this JObject json, string field, T defult)
    {
        try
        {
            return json.GetApiValue<T>(field, true);
        }
        catch
        {
            return defult;
        }
    }

    /// <summary>
    /// 获取json值,非基类,若取不到,返回空或者指定错误
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="field"></param>
    /// <param name="need"></param>
    /// <returns></returns>
    public static T GetApiObject<T>(this JObject json, string field, bool need = false)
    {
        try
        {
            if (json.Property(field, StringComparison.OrdinalIgnoreCase) == null)
            {
                if (need) throw new Exception();
                else return default;
            }
            return json.GetValue(field, StringComparison.OrdinalIgnoreCase).ToString().ToObject<T>();
        }
        catch
        {
            if (need) throw new ApiException("参数有误 请填写正确的" + field);
            return default;
        }
    }

    /// <summary>
    /// 获取Json数组值,若取不到,返回空或者指定错误
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="field"></param>
    /// <param name="need"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetApiValues<T>(this JObject json, string field, bool need = false)
    {
        try
        {
            if (json.Property(field, StringComparison.OrdinalIgnoreCase) == null)
            {
                if (need) throw new Exception();
                else return new List<T>();
            }
            return json.GetValue(field, StringComparison.OrdinalIgnoreCase).Values<T>();
        }
        catch
        {
            if (need) throw new ApiException("参数有误 请填写正确的" + field);
            return new List<T>();
        }
    }
}