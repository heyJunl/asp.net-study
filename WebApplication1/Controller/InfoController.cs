/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;
using WebApplication1.Entity;
using WebApplication1.Service;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
public class InfoController : ControllerBase
{
    private readonly IStudentService _studentService;

    private readonly InfoContext _info;
    
    public InfoController(IStudentService studentService, InfoContext info)
    {
        this._studentService = studentService;
        this._info = info;

    }

    [HttpPost("Add")]
    public async Task<ActionResult<String>> Add(Student student)
    {
        return Ok(await _studentService.Add(student));
    }

    [HttpPost("Update")]
    public async Task<ActionResult<String>> Update(Student student)
    {
        return Ok(await _studentService.Update(student));
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<String>> Delete(Student student)
    {
        return Ok(await _studentService.Delete(student));
    }

    [HttpPost("Query")]
    public async Task<ActionResult<IEnumerable<Student>>> Query()
    {
        // return Ok(await _info.Student.ToListAsync());
        return Ok(await _studentService.Query());
    }

    [HttpPost("QueryById")]
    [Authorize(Policy = "Permission")]
    // [Authorize(Roles = "Permission")]
    public async Task<ActionResult<Student>> QueryById(string id)
    {
        return Ok(await _studentService.QueryById(id));
    }

    [HttpPost("Page")]
    public async Task<ActionResult<PaginatedResponse<Student>>> Page(Student student)
    {
        return Ok(await _studentService.Page(student));
    }
}