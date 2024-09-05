/*
 * @Author: Jun
 * @Description:
 */

using WebApplication1.Entity;

namespace WebApplication1.Vo;

public class PageUserVo
{
    public string Id { get; set; }
    public string Username { get; set; }
    public int Permission { get; set; }
    public int State { get; set; }


    public PageUserVo()
    {
        
    }
}