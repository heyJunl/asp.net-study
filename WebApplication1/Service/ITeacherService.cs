using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entity;

namespace WebApplication1.Service;

public interface ITeacherService
{
    public Task<ActionResult<string>> Add(Teacher teacher);

    public Task<ActionResult<string>> Delete(string id);

    public Task<ActionResult<string>> Update(string id);
    
}