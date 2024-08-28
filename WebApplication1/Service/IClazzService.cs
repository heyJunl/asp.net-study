using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Service;

public interface IClazzService
{
    public Task<ActionResult<string>> Add(ClazzAddDto clazz);

    public Task<ActionResult<string>> Delete(string id);

    public Task<ActionResult<PaginatedResponse<ClazzPageVo>>> Page(ClazzPageDto dto);

    public Task<ActionResult<string>> Update(ClazzUpdateDto dto);

    public Task<ActionResult<Clazz>> Query(string id);
}