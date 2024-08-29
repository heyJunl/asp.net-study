using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entity;

namespace WebApplication1.Service;

public interface IStudentService
{
    public Task<ActionResult<String>> Add(Student student);
    public Task<ActionResult<String>> Update(Student student);
    public Task<ActionResult<String>> Delete(Student student);
    public Task<ActionResult<IEnumerable<Student>>> Query();
    public Task<ActionResult<Student>> QueryById(string id);
    public Task<ActionResult<PaginatedResponse<Student>>> Page(Student student);
    public Task<ActionResult<List<Student>>> QueryClassmate(string id);
}