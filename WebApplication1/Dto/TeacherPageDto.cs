/*
 * @Author: Jun
 * @Description:
 */

using WebApplication1.Entity;

namespace WebApplication1.Dto;

public class TeacherPageDto: PageParam
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int? State { get; set; }
}