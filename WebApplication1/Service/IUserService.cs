using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Service;

public interface IUserService
{
    public Task<ActionResult<string>> Add(User user);

    public Task<ActionResult<string>> Login(User user);

    public Task<ActionResult<PaginatedResponse<UserPageVo>>> QueryUserPage(QueryUserPage queryUserPage);

    public Task<ActionResult<String>> Update(UserUpdateDto userUpdateDto);

}