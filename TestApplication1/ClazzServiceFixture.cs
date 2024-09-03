/*
 * @Author: Jun
 * @Description:
 */

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication1.DbContexts;
using WebApplication1.Service;
using WebApplication1.Service.Impl;

namespace TestApplication1;

public class ClazzServiceFixture : IDisposable
{
    public InfoContext InfoContext { get; private set; }
    public IClazzService ClazzService { get; private set; }

    
    public ClazzServiceFixture()
    {
        // 获取连接字符串
        var connectionString = "Server=localhost;Database=information;Uid=root;Pwd=123456;";

        var options = new DbContextOptionsBuilder<InfoContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information).Options;
        

        // 创建数据库上下文实例
        InfoContext = new InfoContext(options);

        // 假设你有一个方法来创建IClazzService实例
        ClazzService = CreateClazzService(InfoContext);
    }

    private IClazzService CreateClazzService(InfoContext context)
    {
        var connectionString = "Server=localhost;Database=information;Uid=root;Pwd=123456;";
        // 这里你需要根据实际情况创建IClazzService实例
        // 例如，你可以使用依赖注入容器来获取服务实例
        var serviceProvider = new ServiceCollection()
            .AddScoped<IClazzService, ClazzServiceImpl>()
            .AddDbContext<InfoContext>(options =>
            {
                options.UseMySql(connectionString, new MySqlServerVersion("8.0.39"));
                options.EnableSensitiveDataLogging();
            })
            .BuildServiceProvider();
        return serviceProvider.GetRequiredService<IClazzService>();
    }

    public void Dispose()
    {
        InfoContext?.Dispose();
    }
}