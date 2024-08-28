/*
 * @Author: Jun
 * @Description:
 */

using System.Runtime.InteropServices;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Common.Enums;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Utils;
using WebApplication1.Vo;

namespace WebApplication1.Service.Impl;

public class UserServiceImpl : IUserService
{
    private readonly InfoContext _info;
    private readonly JwtUtils _jwt;
    private readonly RedisUtils _redis;
    private readonly IMapper _mapper;


    public UserServiceImpl(InfoContext info, JwtUtils jwt, RedisUtils redis, IMapper mapper)
    {
        this._info = info;
        this._jwt = jwt;
        this._redis = redis;
        this._mapper = mapper;
    }

    public async Task<ActionResult<string>> Add(User user)
    {
        if (_info.User.FirstOrDefaultAsync(e => e.Username == user.Username).Result != null)
        {
            throw new Exception("用户名已存在");
        }

        var salt = GenerateSalt();
        var pwdHash = GeneratePassword(user.Pwd, salt);
        var saveUser = new User(user.Username, Convert.ToBase64String(salt), pwdHash);
        await _info.User.AddAsync(saveUser);
        await _info.SaveChangesAsync();
        return "添加成功";
    }

    public async Task<ActionResult<string>> Login(User user)
    {
        var userInfo = _info.User.FirstOrDefaultAsync(e => e.Username == user.Username).Result;
        if (userInfo == null)
        {
            throw new Exception("用户名不存在");
        }

        byte[] salt = Convert.FromBase64String(userInfo.Salt);
        string pwdHash = GeneratePassword(user.Pwd, salt);
        if (userInfo.Pwd == pwdHash && userInfo.State == StateType.ACTIVATE.GetHashCode())
        {
            var token = _jwt.GenerateToken(userInfo);
            var userToken = new UserToken(userInfo.Id, userInfo.Username, userInfo.Permission);
            var userJson = JsonConvert.SerializeObject(userToken);
            _redis.GetDatabase().StringSet("TOKEN_" + token, userJson, TimeSpan.FromDays(1));

            return $"登录成功，token = {token}";
        }
        else if (userInfo.Pwd == pwdHash && userInfo.State == (int)StateType.DEACTIVATE)
        {
            return "账号冻结";
        }
        else
        {
            return "密码错误";
        }
    }


    public async Task<ActionResult<PaginatedResponse<UserPageVo>>> QueryUserPage(QueryUserPage query)
    {
        var totalCount = await _info.User.CountAsync();
        var page = new PageParam(query.PageNo.Value, query.PageSize.Value, totalCount);
        var wrapper = _info.User.AsQueryable();
        if (!string.IsNullOrEmpty(query.Username))
        {
            wrapper.Where(e => e.Username.Contains(query.Username));
        }

        if (query.State != null)
        {
            wrapper.Where(e => e.State == query.State);
        }

        if (query.Permission != null)
        {
            wrapper.Where(e => e.Permission == query.State);
        }

        List<User> listAsync = await wrapper.Skip((page.PageNo.Value - 1) * page.PageSize.Value)
            .Take(page.PageSize.Value).OrderBy(e => e.CreateTime).ToListAsync();
        List<UserPageVo> result = _mapper.Map<List<UserPageVo>>(listAsync);
        return new PaginatedResponse<UserPageVo>(result, page);
    }

    public async Task<ActionResult<String>> Update(UserUpdateDto dto)
    {
        var result = await _info.User.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == dto.Id);
        if (result == null)
        {
            return "用户不存在";
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(dto.Pwd))
            {
                var salt = GenerateSalt();
                var pwdHash = GeneratePassword(dto.Pwd, salt);
                result = _mapper.Map<User>(dto);
                result.Pwd = pwdHash;
                result.Salt = Convert.ToBase64String(salt);
                _info.Update(result);
                await _info.SaveChangesAsync();
                return "更新成功";
            }
            else
            {
                string pwd = result.Pwd;
                string salt = result.Salt;
                
                // _mapper.Map(dto, result); 这个写法在tracking的时候可成功
                result = _mapper.Map<User>(dto);    // 这个写法在tracking的情况下会id重复
                result.Pwd = pwd;
                result.Salt = salt;
                _info.Update(result);
                // _info.Entry(result).State = EntityState.Modified;
                await _info.SaveChangesAsync();
                return "更新成功";
            }
        }
    }

    public async Task<ActionResult<string>> Delete(string id)
    {
        var user = await _info.User.FirstOrDefaultAsync(e => e.Id == id);
        if (user == null)
        {
            throw new ArgumentException("用户不存在");
        }
        user.State = StateType.DEACTIVATE.GetHashCode();
        await _info.SaveChangesAsync();
        return "删除成功";
    }





    public async Task<ActionResult<UserPageVo>> QueryById(string id)
    {
        var user = await _info.User.FirstOrDefaultAsync(e => e.Id == id);
        if (user == null)
        {
            throw new ArgumentException("用户不存在");
        }

        var result = _mapper.Map<UserPageVo>(user);
        return result;
    }
    
    
    
    
    
    

    /**
     * 生成盐
     */
    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        return salt;
    }

    /**
     * 根据密码和盐生成哈希值
     */
    public string GeneratePassword(string password, byte[] salt)
    {
        using (var p = new Rfc2898DeriveBytes(password, salt, 10000))
        {
            byte[] hash = p.GetBytes(20);
            return Convert.ToBase64String(hash);
        }
    }
}