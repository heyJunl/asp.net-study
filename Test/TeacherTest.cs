/*
 * @Author: Jun
 * @Description:
 */

using WebApplication1.Entity;
using WebApplication1.Service;

namespace Test;

public class TeacherTest
{
    private readonly ITeacherService _teacherService;

    public TeacherTest(ITeacherService teacher)
    {
        _teacherService = teacher;
    }

    [Fact]
    public async Task Add()
    {
        var teacher = new Teacher() { Name = "张三", Phone = "12345678901", Sex = 0 };
        await _teacherService.Add(teacher);
        var task = _teacherService.Query(teacher.Id);
        Assert.NotNull(task);
        Assert.Equal("张三", teacher.Name);
    }
}