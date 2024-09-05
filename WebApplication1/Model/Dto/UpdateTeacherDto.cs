/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Dto;

public class UpdateTeacherDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public int Sex { get; set; }
    public string? Phone { get; set; }
    public int? State { get; set; } = 0;
}