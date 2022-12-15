using System.Reflection;
using System.Text;

namespace Util;

public static class CSVHelper
{
    /// <summary>
    /// 保存到CSV流
    /// </summary>
    /// <param name="dataList">data source</param>
    /// <returns>success flag</returns>
    public static Stream SaveDataToCSVFile<T>(List<T> dataList) where T : class
    {
        StringBuilder sb_Text = new();
        StringBuilder strColumn = new();
        StringBuilder strValue = new();
        Type tp = typeof(T);
        PropertyInfo[] props = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (int i = 0; i < props.Length; i++)
        {
            PropertyInfo itemPropery = props[i];
            if (itemPropery.GetCustomAttributes(typeof(AttrForCsvColumnLabel), true).FirstOrDefault() is AttrForCsvColumnLabel labelAttr)
                strColumn.Append(labelAttr.Title);
            else
                strColumn.Append(props[i].Name);

            strColumn.Append(',');
        }
        strColumn.Remove(strColumn.Length - 1, 1);
        sb_Text.AppendLine(strColumn.ToString());

        for (int i = 0; i < dataList.Count; i++)
        {
            T model = dataList[i];
            strValue.Clear();
            for (int m = 0; m < props.Length; m++)
            {
                PropertyInfo itemPropery = props[m];
                object val = itemPropery.GetValue(model, null);
                if (m == 0)
                {
                    strValue.Append(val);
                }
                else
                {
                    strValue.Append(',');
                    strValue.Append(val);
                }
            }
            sb_Text.AppendLine(strValue.ToString());
        }

        byte[] array = Encoding.UTF8.GetBytes(sb_Text.ToString());
        return new MemoryStream(array);
    }
}

/// <summary>
/// 标记属性的别名Title
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class AttrForCsvColumnLabel : Attribute
{
    public string Title { get; set; }
}