/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.DbContexts;
using WebApplication1.Service;
using WebApplication1.Service.Impl;
using WebApplication1.Utils;

namespace TestApplication1;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TestService(this IServiceCollection service)
    {
        service.AddScoped<IClazzService, ClazzServiceImpl>();
        service.AddScoped<IStudentService, StudentServiceImpl>();
        service.AddScoped<IUserService, UserServiceImpl>();
        service.AddScoped<ITeacherService, TeacherServiceImpl>();
        service.AddAutoMapper(typeof(MappingProfile).Assembly);
        service.AddDbContext<InfoContext>(opt =>
        {
            opt.UseMySql("Server=localhost;Database=information;Uid=root;Pwd=123456;",
                new MySqlServerVersion("8.0.39"));
            opt.EnableSensitiveDataLogging();
        });
        return service;
    }
}