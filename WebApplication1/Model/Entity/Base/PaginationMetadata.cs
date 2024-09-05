/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Entity;

public class PaginatedResponse<T>
{
    public PageParam Meta { get; set; }
    public IEnumerable<T> Data { get; set; }

    public PaginatedResponse(IEnumerable<T> data, PageParam meta)
    {
        Meta = meta;
        Data = data;
    }

    public PaginatedResponse()
    {
        
    }
}
