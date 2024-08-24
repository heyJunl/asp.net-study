/*
 * @Author: Jun
 * @Description:
 */

using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Common.Enums;
using WebApplication1.DbContexts;
using WebApplication1.Entity;
using WebApplication1.Utils;

namespace WebApplication1.Service.Impl;

public class UserServiceImpl : IUserService
{
    private readonly InfoContext _info;
    private readonly JwtUtils _jwt;
    private readonly RedisUtils _redis;
    

    public UserServiceImpl(InfoContext info, JwtUtils jwt, RedisUtils redis)
    {
        this._info = info;
        this._jwt = jwt;
        this._redis = redis;
    }
    
    public async Task<ActionResult<string>> Add(User user)
    {
        if (_info.User.FirstOrDefaultAsync(e => e.Username == user.Username).Result !=null)
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
            
            return$"登录成功，token = {token}";
        }
        else if (userInfo.Pwd != pwdHash && userInfo.State == StateType.ACTIVATE.GetHashCode())
        {
            return "账号冻结";
        } else
        {
            return "密码错误";
        }
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