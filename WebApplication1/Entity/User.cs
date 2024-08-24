/*
 * @Author: Jun
 * @Description:
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Utils;

namespace WebApplication1.Entity;

public class User : BaseData
{
    [Key] public string? Id { get; set; } = new SnowFlake(1, 1).NextId().ToString();
    public string Username { get; set; }
    public string Pwd { get; set; }
    public int Permission { get; set; } = 0;
    public int State { get; set; } = 0;
    public string? Salt { get; set; }

    public User()
    {
        
    }

    public User(string username, string salt, string pwd)
    {
        this.Username = username;
        this.Salt = salt;
        this.Pwd = pwd;
    }

    public User(string id, string username, int permission)
    {
        this.Id = id;
        this.Username = username;
        this.Permission = permission;
    }

}