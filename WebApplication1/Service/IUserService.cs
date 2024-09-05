using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Service;

public interface IUserService
{
    // 添加用户
    public Task<ActionResult<string>> Add(User user);

    // 登录
    public Task<ActionResult<string>> Login(User user);

    // 分页查询，根据username，permission，state模糊查询
    public Task<ActionResult<PaginatedResponse<PageUserVo>>> QueryUserPage(QueryUserPage queryUserPage);

    // 修改用户信息
    public Task<ActionResult<string>> Update(UpdateUserDto updateUserDto);

    // 冻结用户
    public Task<ActionResult<string>> Delete(string id);

    public Task<ActionResult<PageUserVo>> QueryById(string id);
}