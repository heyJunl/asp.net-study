/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Dto;

public class AddClazzUpdateDto : AddClazzDto
{
    public string? Id { get; set; }
    public string? Grade { get; set; }
    public string? Number { get; set; }
    public string? Year { get; set; }
    public string? Room { get; set; }
    public string TeacherId { get; set; }
    public int Sub { get; set; }
}