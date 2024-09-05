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
    public async Task<IResult> Add(AddClazzDto addClazz)
    {
        return TypedResults.Ok(await _clazzService.Add(addClazz));
    }

    /**
     * 冻结班级
     */
    [HttpPost("Delete")]
    public async Task<IResult> Delete(string id)
    {
        return TypedResults.Ok(await _clazzService.Delete(id));
    }

    /**
     * 分页模糊查询
     */
    [HttpPost("Page")]
    public async Task<IResult> Page(PageClazzDto dto)
    {
        return TypedResults.Ok(await _clazzService.Page(dto));
    }
    
    /**
     * 更新班级信息
     */
    [HttpPost("Update")]
    public async Task<IResult> Update(AddClazzUpdateDto addClazz)
    {
        return TypedResults.Ok(await _clazzService.Update(addClazz));
    }

    /**
     * 查询班级信息
     */
    [HttpPost("Query")]
    public async Task<IResult> Query(string id)
    {
        return TypedResults.Ok(await _clazzService.Query(id));
    }

    [HttpPost("QueryGradeStudent")]
    public async Task<IResult> QueryClazzStudent(string clazzId)
    {
        return TypedResults.Ok(await _clazzService.QueryClazzStudent(clazzId));
    }
}