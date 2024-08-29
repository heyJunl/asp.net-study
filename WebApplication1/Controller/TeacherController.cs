/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Service;
using WebApplication1.Utils;
using WebApplication1.Vo;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
public class TeacherController: ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost("Add")]
    public async Task<ActionResult<string>> Add(Teacher teacher)
    {
        return Ok(await _teacherService.Add(teacher));
    }

    [HttpPost("Delete")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        return Ok(await _teacherService.Delete(id));
    }

    [HttpPost("Update")]
    public async Task<ActionResult<string>> Update(Teacher teacher)
    {
        return Ok(await _teacherService.Update(teacher));
    }

    [HttpPost("Page")]
    public async Task<ActionResult<PaginatedResponse<Teacher>>> Page(TeacherPageDto dto)
    {
        return Ok(await _teacherService.QueryTeacherPage(dto));
    }

    [HttpPost("Query")]
    public async Task<ActionResult<Teacher>> Query(string id)
    {
        return Ok(await _teacherService.Query(id));
    }

    [HttpPost("EntrustWork")]
    public async Task<ActionResult<string>> EntrustWork(string outId, string inId)
    {
        
        return Ok(await _teacherService.EntrustWork(outId, inId));
    }
    
    [HttpPost("ChargedWork")]
    public async Task<ActionResult<List<StudentClazzVo>>> ChargedWork(string id)
    {
        return Ok(await _teacherService.ChargedWork(id));
    }
}