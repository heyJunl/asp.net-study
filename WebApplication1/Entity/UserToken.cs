/*
 * @Author: Jun
 * @Description:
 */

namespace WebApplication1.Entity;

public class UserToken
{
    public string Id;
    public string Username;
    public int Permission;

    public UserToken(string id, string username, int permission)
    {
        Id = id;
        Username = username;
        Permission = permission;
    }
}