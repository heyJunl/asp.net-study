/*
 * @Author: Jun
 * @Description:
 */

using WebApplication1.Entity;

namespace WebApplication1.Dto;

public class QueryUserPage: PageParam
{
    public string? Username { get; set; }
    public int? Permission { get; set; }
    public int? State { get; set; }

    public QueryUserPage()
    {
        
    }
    
    
}