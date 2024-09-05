using Microsoft.AspNetCore.Mvc;
using WebApplication1.Common;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Service;

public interface IClazzService
{
    public Task<ActionResult<string>> Add(AddClazzDto addClazz);

    public Task<ActionResult<string>> Delete(string id);

    public Task<ActionResult<PaginatedResponse<PageClazzVo>>> Page(PageClazzDto dto);

    public Task<ActionResult<string>> Update(AddClazzUpdateDto dto);

    public Task<ActionResult<Clazz>> Query(string id);

    public Task<ActionResult<List<GradeStudentVo>>> QueryClazzStudent(string clazzId);
}