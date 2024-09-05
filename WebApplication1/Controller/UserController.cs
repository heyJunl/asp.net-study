/*
 * @Author: Jun
 * @Description:
 */

using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalApis.Exception;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Service;
using WebApplication1.Utils;
using WebApplication1.Vo;

namespace WebApplication1.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Admins")]
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
    public async Task<IResult> Add(User user)
    {
        return TypedResults.Ok(await _userService.Add(user));
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IResult> Login(User user)
    {
        return TypedResults.Ok(await _userService.Login(user));
    }
    
    [AllowAnonymous]
    [HttpPost("Logout")]
    public async Task<IResult> Logout()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (String.IsNullOrWhiteSpace(token))
        {
            throw new CustomException(HttpStatusCode.InternalServerError.GetHashCode(), "请携带Token");
        }

        string[] tokenList = token.Split(" ");
        if (tokenList.Length != 2 || String.IsNullOrWhiteSpace(tokenList[1]))
        {
            throw new CustomException(HttpStatusCode.InternalServerError.GetHashCode(), "无效的Token");
        }
        token = "TOKEN_" + tokenList[1];
        _redis.GetDatabase().KeyDelete(token);
        return TypedResults.Ok("删除成功！");
    }

    [HttpPost("Page")]
    public async Task<IResult> Page(QueryUserPage queryUserPage)
    {
        return TypedResults.Ok(await _userService.QueryUserPage(queryUserPage));
    }

    [HttpPost("Update")]
    public async Task<IResult> Update(UpdateUserDto dto)
    {
        return TypedResults.Ok(await _userService.Update(dto));
    }

    [HttpPost("Delete")]
    public async Task<IResult> Delete(string id)
    {
        return TypedResults.Ok(await _userService.Delete(id));
    }

    [HttpPost("QueryById")]
    public async Task<IResult> QueryById(string id)
    {
        return TypedResults.Ok(await _userService.QueryById(id));
    }
    
    
}