/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Dto;

public class UpdateUserDto
{
 public string Id { get; set; }
 public string? Username { get; set; }
 public string?  Pwd { get; set; }
 public int Permission { get; set; } = 0;
 public int State { get; set; } = 0;

}