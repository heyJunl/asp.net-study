using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Service;

public interface ITeacherService
{
    public Task<ActionResult<string>> Add(Teacher teacher);

    public Task<ActionResult<string>> Delete(string id);

    public Task<ActionResult<string>> Update(Teacher teacher);

    public Task<ActionResult<PaginatedResponse<Teacher>>> QueryTeacherPage(TeacherPageDto dto);

    public Task<ActionResult<Teacher>> Query(string id);

    public Task<ActionResult<string>> EntrustWork(string outId, string inId);

    public Task<ActionResult<List<StudentClazzVo>>> ChargedWork(string id);

}