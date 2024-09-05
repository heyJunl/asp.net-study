/*
 * @Author: Jun
 * @Description:
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Utils;

namespace WebApplication1.Entity;

public class Student : BaseData
{
    [Key] public string Id { get; set; } = new SnowFlake(1, 1).NextId().ToString();
    public string? Name { get; set; }
    public int? Sex { get; set; }
    [Column("class_id")]
    public string ClassId { get; set; }
    public string Birth { get; set; }
    public string Address { get; set; }
    public string Dept { get; set; }
}