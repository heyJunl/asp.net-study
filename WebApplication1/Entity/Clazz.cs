/*
 * @Author: Jun
 * @Description:
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Utils;

namespace WebApplication1.Entity;

public class Clazz : BaseData
{
    [Key] public string Id { get; set; } = new SnowFlake(1, 1).NextId().ToString();
    public string? Grade { get; set; }
    public string? Number { get; set; }
    public string? Year { get; set; }
    public string? Room { get; set; }
    [Column("teacher_id")]
    public string? TeacherId { get; set; }

    public int? Total { get; set; } = 0;
    public int? Sub { get; set; } = 0;
    public int State { get; set; } = 0;
}