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
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"] ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("Id", user.Id),
            new Claim("Username", user.Username),
            new Claim("Role", "User"),
            new Claim("User", "1"),
            new Claim(ClaimTypes.Role, "Permission"),
            new Claim("Issuer", _configuration["Jwt:Issuer"]),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
            new Claim("NotBefore", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")),
            new Claim(JwtRegisteredClaimNames.Exp, (DateTime.Now.AddDays(1).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds.ToString()), // 添加 exp claim
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        Console.WriteLine(token);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}