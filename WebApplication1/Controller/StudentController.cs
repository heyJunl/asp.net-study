/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Service;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    private readonly InfoContext _info;
    
    public StudentController(IStudentService studentService, InfoContext info)
    {
        this._studentService = studentService;
        this._info = info;

    }

    [HttpPost("Add")]
    public async Task<ActionResult<string>> Add(Student student)
    {
        return Ok(await _studentService.Add(student));
    }

    [HttpPost("Update")]
    public async Task<ActionResult<string>> Update(UpdateStudentDto student)
    {
        return Ok(await _studentService.Update(student));
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        return Ok(await _studentService.Delete(id));
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

    [HttpPost("QueryClassmate")]
    public async Task<ActionResult<Student>> QueryClassmate(string id)
    {
        return Ok(await _studentService.QueryClassmate(id));
    }

    
}