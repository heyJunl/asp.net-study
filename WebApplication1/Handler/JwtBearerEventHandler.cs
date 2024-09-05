/*
 * @Author: Jun
 * @Description:
 */

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication1.Handler;

public class JwtBearerEventHandler: JwtBearerEvents
{
    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        await base.AuthenticationFailed(context);
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var token = context.SecurityToken as JwtSecurityToken;
        if (token != null)
        {
            var claims = token.Claims.ToList();
            foreach (var claim in claims)
            {
                context.HttpContext.Request.Headers.Add(claim.Type, claim.Value);
            }
        }

        await base.TokenValidated(context);
    }
}