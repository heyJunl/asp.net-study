using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Entity;

namespace WebApplication1.Service;

public interface IStudentService
{
    public Task<ActionResult<string>> Add(Student student);
    public Task<ActionResult<string>> Update(UpdateStudentDto student);
    public Task<ActionResult<string>> Delete(string id);
    public Task<ActionResult<IEnumerable<Student>>> Query();
    public Task<ActionResult<Student>> QueryById(string id);
    public Task<ActionResult<PaginatedResponse<Student>>> Page(Student student);
    public Task<ActionResult<List<Student>>> QueryClassmate(string id);
}