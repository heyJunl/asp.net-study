/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entity;
using WebApplication1.Service;
using WebApplication1.Utils;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    private readonly RedisUtils _redis;

    public UserController(IUserService userService, RedisUtils redis)
    {
        this._userService = userService;
        this._redis = redis;
    }

    [HttpPost("Add")]
    public async Task<ActionResult<string>> Add(User user)
    {
        return Ok(await _userService.Add(user));
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(User user)
    {
        return Ok(await _userService.Login(user));
    }
    
    [HttpPost("Logout")]
    public async Task<ActionResult<string>> Logout()
    {
        var token = "TOKEN_" + HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
        _redis.GetDatabase().KeyDelete(token);
        return Ok("删除成功！");
    }
}