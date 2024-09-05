/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Service;
using WebApplication1.Utils;
using WebApplication1.Vo;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Permission")]
public class TeacherController: ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost("Add")]
    public async Task<IResult> Add(Teacher teacher)
    {
        return TypedResults.Ok(await _teacherService.Add(teacher));
    }

    [HttpPost("Delete")]
    public async Task<IResult> Delete(string id)
    {
        return TypedResults.Ok(await _teacherService.Delete(id));
    }

    [HttpPost("Update")]
    public async Task<IResult> Update(UpdateTeacherDto teacher)
    {
        return TypedResults.Ok(await _teacherService.Update(teacher));
    }

    [HttpPost("Page")]
    public async Task<IResult> Page(PageTeacherDto dto)
    {
        return TypedResults.Ok(await _teacherService.QueryTeacherPage(dto));
    }

    [HttpPost("Query")]
    public async Task<IResult> Query([FromForm] string id)

    {
        return TypedResults.Ok(await _teacherService.Query(id));
    }

    [HttpPost("EntrustWork")]
    public async Task<IResult> EntrustWork(string outId, string inId)
    {
        
        return TypedResults.Ok(await _teacherService.EntrustWork(outId, inId));
    }
    
    [HttpPost("ChargedWork")]
    public async Task<IResult> ChargedWork(string id)
    {
        return TypedResults.Ok(await _teacherService.ChargedWork(id));
    }
}