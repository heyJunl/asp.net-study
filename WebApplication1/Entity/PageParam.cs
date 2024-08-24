/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Entity;

public class PageParam
{
    public int? PageNo { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public int? TotalCount { get; set; }

    public PageParam(int pageNo, int pageSize, int totalCount)
    {
        if (pageSize>100)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize not greater than 100");
        }
        this.PageSize = pageSize;
        this.PageNo = pageNo;
        this.TotalCount = totalCount;
    }

    public PageParam()
    {
        
    }
}