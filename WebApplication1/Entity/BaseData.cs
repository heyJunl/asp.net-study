/*
 * @Author: Jun
 * @Description:
 */

using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entity;

public class BaseData
{
    [Column("create_time")]
    public DateTime CreateTime { get; set; } //= DateTime.Now;
    [Column("update_time")]
    public DateTime UpdateTime { get; set; } //= DateTime.Now;
    [Column("create_by")]
    public string? CreateBy { get; set; }
    [Column("update_by")]
    public string? UpdateBy { get; set; }

    [NotMapped]
    public PageParam? PageParam { get; set; }

}