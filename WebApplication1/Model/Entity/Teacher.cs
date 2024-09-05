/*
 * @Author: Jun
 * @Description:
 */

using System.ComponentModel.DataAnnotations;
using WebApplication1.Utils;

namespace WebApplication1.Entity;

public class Teacher : BaseData
{
    [Key] public string Id { get; set; } = new SnowFlake(1, 1).NextId().ToString();
    public string? Name { get; set; }
    public int Sex { get; set; }
    public string? Phone { get; set; }
    public int? State { get; set; } = 0;
}