/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.DbContexts;

namespace Test;

public class Startup
{
    public Startup()
    {
        
    }
    

    public IConfiguration Configuration { get; }
    

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers();

        // 配置数据库
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<InfoContext>(opt =>
        {
            opt.UseMySql(connectionString, new MySqlServerVersion("8.0.39"));
            opt.EnableSensitiveDataLogging();
        });

        // Add AutoMapper services.
        services.AddAutoMapper(typeof(Startup));

        // Add other services...
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    // {
        // if (env.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        // }
        // else
        // {
        //     app.UseExceptionHandler("/Error");
        //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //     app.UseHsts();
        // }
        //
        // app.UseHttpsRedirection();
        // app.UseStaticFiles();
        //
        // app.UseRouting();
        //
        // app.UseAuthorization();
        //
        // app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    // }
}