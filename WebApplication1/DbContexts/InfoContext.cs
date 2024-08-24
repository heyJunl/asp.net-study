/*
 * @Author: Jun
 * @Description:
 */
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entity;


namespace WebApplication1.DbContexts;

public class InfoContext: DbContext
{
    public DbSet<Clazz> Clazz { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<PageParam> PageParam { get; set; }

    public InfoContext(DbContextOptions<InfoContext> options) : base(options)
    {
        
    }

    public InfoContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<User_Test>().Ignore(e => e.aa).Ignore(e=>e.bb);//aa和bb为模型中有数据库中没有的字段，即要忽略的字段
        modelBuilder.Entity<PageParam>(e => e.HasNoKey());  // 忽略映射
    }
}