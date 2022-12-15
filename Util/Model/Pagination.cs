namespace Util;

/// <summary>
/// 数据表格分页
/// </summary>
public class Pagination
{
    public Pagination()
    {
        PageNumber = 1;
        PageSize = int.MaxValue;
    }

    /// <summary>
    /// 当前页码
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// 每页行数
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总记录数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int Pages
    {
        get
        {
            int pages = Total / PageSize;
            int pageCount = Total % PageSize == 0 ? pages : pages + 1;
            return pageCount;
        }
    }

    /// <summary>
    /// 是否首页
    /// </summary>
    public bool IsFirstPage { get => PageNumber == 1; }

    /// <summary>
    /// 是否尾页
    /// </summary>
    public bool IsLastPage { get => PageNumber == Pages; }
}