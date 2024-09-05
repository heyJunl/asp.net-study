/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Dto;

public class UpdateStudentDto
{
 public string Id { get; set; }
 public string? Name { get; set; }
 public int? Sex { get; set; }
 public string ClassId { get; set; }
 public string Birth { get; set; }
 public string Address { get; set; }
 public string Dept { get; set; }
}