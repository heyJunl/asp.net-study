/*
 * @Author: Jun
 * @Description:
 */

using System.Text.Json;

namespace WebApplication1.Vo;

public class PageClazzVo
{
    public string id { get; set; }
    public string? Grade { get; set; }
    public string? Number { get; set; }
    public string? Year { get; set; }
    public string? Room { get; set; }
    public string? TeacherId { get; set; }
    public int Total { get; set; }
    public int Sub { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}