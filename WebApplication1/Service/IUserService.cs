using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entity;

namespace WebApplication1.Service;

public interface IUserService
{
    public Task<ActionResult<string>> Add(User user);

    public Task<ActionResult<string>> Login(User user);
    
}