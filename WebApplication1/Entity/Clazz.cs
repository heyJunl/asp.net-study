/*
 * @Author: Jun
 * @Description:
 */

using System.ComponentModel.DataAnnotations;
using WebApplication1.Utils;

namespace WebApplication1.Entity;

public class Clazz : BaseData
{
    [Key] public string Id { get; set; } = new SnowFlake(1, 1).NextId().ToString();
    public string? Grade { get; set; }
    public string? Number { get; set; }
    public string? Year { get; set; }
    public string? Room { get; set; }
    public int TeacherId { get; set; }
    public int Total { get; set; }
    public int Sub { get; set; }
}