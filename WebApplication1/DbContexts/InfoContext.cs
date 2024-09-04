/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using WebApplication1.Entity;


namespace WebApplication1.DbContexts;

public class InfoContext : DbContext
{
    public DbSet<Clazz> Clazz { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<Teacher> Teacher { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<PageParam> PageParam { get; set; }
    
    private readonly IHttpContextAccessor _accessor;
    
    public InfoContext(DbContextOptions<InfoContext> options, IHttpContextAccessor accessor) : base(options)
    {
        _accessor = accessor;
    }

    public InfoContext()
    {
        
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<User_Test>().Ignore(e => e.aa).Ignore(e=>e.bb);//aa和bb为模型中有数据库中没有的字段，即要忽略的字段
        modelBuilder.Entity<PageParam>(e => e.HasNoKey()); // 忽略映射
    }

    public override int SaveChanges()
    {
        var entityEntries = ChangeTracker.Entries().ToList();
        foreach (var item in entityEntries)
        {
            if (item.State == EntityState.Added)
            {
                Entry(item.Entity).Property(nameof(BaseData.CreateTime)).CurrentValue = DateTime.Now;
            }

            if (item.State == EntityState.Modified)
            {
                Entry(item.Entity).Property(nameof(BaseData.UpdateTime)).CurrentValue = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        var httpContextAccessor = _accessor.HttpContext?.Request?.Headers["Authorization"];
        
        var entityEntries = ChangeTracker.Entries().ToList();
        foreach (var item in entityEntries)
        {
            if (item.State == EntityState.Added)
            {
                Entry(item.Entity).Property(nameof(BaseData.CreateTime)).CurrentValue = DateTime.Now;
            }

            if (item.State == EntityState.Modified)
            {
                Entry(item.Entity).Property(nameof(BaseData.UpdateTime)).CurrentValue = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}