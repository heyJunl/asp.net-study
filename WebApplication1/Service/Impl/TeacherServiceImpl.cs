/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Mvc;
using WebApplication1.DbContexts;
using WebApplication1.Entity;

namespace WebApplication1.Service.Impl;

public class TeacherServiceImpl : ITeacherService
{
    private readonly InfoContext _info;
    
    public TeacherServiceImpl(InfoContext info)
    {
        this._info = info;
    }
    
    public async Task<ActionResult<string>> Add(Teacher teacher)
    {
        await _info.Teacher.AddAsync(teacher);
        await _info.SaveChangesAsync();
        return "添加成功";

    }

    public Task<ActionResult<string>> Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<string>> Update(string id)
    {
        throw new NotImplementedException();
    }
}