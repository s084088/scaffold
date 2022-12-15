namespace Util.Helper;

/// <summary>
/// 特殊字符帮助类
/// </summary>
public class SpecialWordHelper
{
    /// <summary>
    /// 去除特殊字符
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string RemoveSpecialWord(string word)
    {
        var result = word;
        string chars = "abcdefghijklmnopqrstuvwxyz";
        foreach (var item in word.ToCharArray())
        {
            if (!chars.Contains(item))
            {
                result = result.Replace(item.ToString(), "");
            }
        }
        return result;
    }
}