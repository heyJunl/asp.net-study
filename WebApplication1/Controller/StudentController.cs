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
    public async Task<IResult> Add(Student student)
    {
        return TypedResults.Ok(await _studentService.Add(student));
    }

    [HttpPost("Update")]
    public async Task<IResult> Update(UpdateStudentDto student)
    {
        return TypedResults.Ok(await _studentService.Update(student));
    }

    [HttpDelete("Delete")]
    public async Task<IResult> Delete(string id)
    {
        return TypedResults.Ok(await _studentService.Delete(id));
    }

    [HttpPost("Query")]
    public async Task<IResult> Query()
    {
        // return Ok(await _info.Student.ToListAsync());
        return TypedResults.Ok(await _studentService.Query());
    }

    [HttpPost("QueryById")]
    [Authorize(Policy = "Permission")]
    // [Authorize(Roles = "Permission")]
    public async Task<IResult> QueryById(string id)
    {
        return TypedResults.Ok(await _studentService.QueryById(id));
    }

    [HttpPost("Page")]
    public async Task<IResult> Page(Student student)
    {
        return TypedResults.Ok(await _studentService.Page(student));
    }

    [HttpPost("QueryClassmate")]
    public async Task<IResult> QueryClassmate(string id)
    {
        return TypedResults.Ok(await _studentService.QueryClassmate(id));
    }

    
}