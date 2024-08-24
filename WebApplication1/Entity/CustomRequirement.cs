/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Entity;

public class CustomRequirement : IAuthorizationRequirement
{
    public string Permission { get; set; }

    public CustomRequirement(string permission)
    {
        this.Permission = permission;
    }
}