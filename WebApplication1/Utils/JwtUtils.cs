/*
 * @Author: Jun
 * @Description:
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"] ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Role, "Permission"),
                new Claim("Permission", "1"),
                
                // 显式添加 exp 声明
                // new Claim(JwtRegisteredClaimNames.Exp, ((DateTime.UtcNow.AddDays(1) - new DateTime(1970, 1, 1)).TotalSeconds).ToString("F0"))
            },
            expires:DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.Now,
            signingCredentials: credentials);
        
        // var expires = DateTime.Now.AddDays(1).ToUniversalTime();
        // var expClaim = new Claim(JwtRegisteredClaimNames.Exp, ((DateTimeOffset)expires).ToUnixTimeSeconds().ToString());
        // var claims = new[]
        // {
        //     new Claim("Id", user.Id),
        //     new Claim("Username", user.Username),
        //     new Claim(ClaimTypes.Role, "Permission"),
        //     new Claim("Permission", "1"),
        //     expClaim,
        //     new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
        //     new Claim("Issuer", _configuration["Jwt:Issuer"])
        //     
        // };
        //
        // var token = new JwtSecurityToken(
        //     issuer: _configuration["Jwt:Issuer"],
        //     claims:claims,
        //     notBefore: DateTime.Now,
        //     expires: DateTime.Now.AddDays(1).ToUniversalTime(),
        //     signingCredentials: credentials);
        
        
        
        
        
        Console.WriteLine();
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}