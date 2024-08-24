using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DbContexts;
using WebApplication1.Service;
using WebApplication1.Service.Impl;
using WebApplication1.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IStudentService, StudentServiceImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // 添加控制台日志提供者
    loggingBuilder.SetMinimumLevel(LogLevel.Information); // 设置最低日志级别为 Information
    
});

// 配置授权
builder.Services.AddAuthorization(options =>
{
    // 注册自定义授权策略
    options.AddPolicy("Admins", policy => policy.RequireClaim("Permission", "1"));
    options.AddPolicy("Teacher", policy => policy.RequireClaim("Permission", "2"));
    options.AddPolicy("Student", policy => policy.RequireClaim("Permission", "3"));
    options.AddPolicy("Visitor", policy => policy.RequireClaim("Permission", "0"));
    options.AddPolicy("User", policy => policy.RequireClaim("User"));

    // 设置默认策略
    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// 配置Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT授权 (将Bearer {token} 放入Authorization头中)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// 配置数据库
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InfoContext>(opt =>
{
    opt.UseMySql(connectionString, new MySqlServerVersion("8.0.39"));
    opt.EnableSensitiveDataLogging();
});

// 配置JWT身份验证策略
// 单例注入
builder.Services.AddSingleton(new JwtUtils(builder.Configuration)); 
// 配置身份验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,  // 发行人是否合法
            ValidateAudience = true,    // 目标受众是否正确
            ValidateLifetime = true,    // 有效期是否过期
            ValidateIssuerSigningKey = true,    // 签名密钥是否正确
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // 从应用配置中读取发行人
            ValidAudience = builder.Configuration["Jwt:Audience"],  // 从应用配置中读取标识符
            IssuerSigningKey =  
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"] ?? "")),  // 通过应用程序配置中获取密钥字符串
            ClockSkew = TimeSpan.FromSeconds(30),   // 允许的时间偏差
            RequireExpirationTime = true    // JWT是否包含过期时间
        };
    });

// 配置Redis
var section = builder.Configuration.GetSection("Redis:Default");
var _connectionString = section.GetSection("Connection").Value ?? "";
var _instanceName = section.GetSection("InstanceName").Value ?? "";
int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
builder.Services.AddSingleton(new RedisUtils(_connectionString, _instanceName, _defaultDB));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    if (context.Request.Query.TryGetValue("access_token", out var accessToken))
    {
        context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
    }

    await next();
});


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapControllers();
app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

