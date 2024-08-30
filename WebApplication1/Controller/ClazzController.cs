/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Service;
using WebApplication1.Vo;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
public class ClazzController : ControllerBase
{
    private readonly IClazzService _clazzService;

    public ClazzController(IClazzService clazzService)
    {
        this._clazzService = clazzService;
    }

    /*
     * 添加班级
     */
    [HttpPost("Add")]
    public async Task<ActionResult<string>> Add(ClazzAddDto clazz)
    {
        return Ok(await _clazzService.Add(clazz));
    }

    /**
     * 冻结班级
     */
    [HttpPost("Delete")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        return Ok(await _clazzService.Delete(id));
    }

    /**
     * 分页模糊查询
     */
    [HttpPost("Page")]
    public async Task<ActionResult<PaginatedResponse<ClazzPageVo>>> Page(ClazzPageDto dto)
    {
        return Ok(await _clazzService.Page(dto));
    }
    
    /**
     * 更新班级信息
     */
    [HttpPost("Update")]
    public async Task<ActionResult<string>> Update(ClazzUpdateDto clazz)
    {
        return Ok(await _clazzService.Update(clazz));
    }

    /**
     * 查询班级信息
     */
    [HttpPost("Query")]
    public async Task<ActionResult<Clazz>> Query(string id)
    {
        return Ok(await _clazzService.Query(id));
    }

    [HttpPost("QueryGradeStudent")]
    public async Task<ActionResult<List<GradeStudentVo>>> QueryGradeStudent(string clazzId)
    {
        return Ok(await _clazzService.QueryGradeStudent(clazzId));
    }
}