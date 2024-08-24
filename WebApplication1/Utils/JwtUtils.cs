/*
 * @Author: Jun
 * @Description:
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Entity;

namespace WebApplication1.Utils;

public class JwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        // 创建数组存储用户相关信息
        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, user.Id),
            new Claim(ClaimTypes.Surname, user.Username),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("User", "1")
        };
        // 创建对称加密密钥，并创建签名凭证
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"] ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],   // 发行人
            _configuration["Jwt:Audience"], // 受众
            claims,
            expires: DateTime.Now.AddDays(1),   // 过期时间 
            signingCredentials: credentials);   // 签名凭证
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}