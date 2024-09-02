using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApplication1.DbContexts;
using WebApplication1.Handler;
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
builder.Services.AddScoped<IClazzService, ClazzServiceImpl>();
builder.Services.AddScoped<ITeacherService, TeacherServiceImpl>();
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
    // options.AddPolicy("User", policy => policy.RequireClaim("User"));
    options.AddPolicy("Permission", policy => policy.RequireClaim("Permission"));
    options.AddPolicy("User", policy => policy.RequireClaim("User"));
    options.AddPolicy("Role", policy => policy.RequireClaim("Role"));


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
            ValidateIssuer = true, // 发行人是否合法
            ValidateAudience = true, // 目标受众是否正确
            ValidateLifetime = true, // 有效期是否过期
            ValidateIssuerSigningKey = true, // 签名密钥是否正确
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // 从应用配置中读取发行人
            ValidAudience = builder.Configuration["Jwt:Audience"], // 从应用配置中读取标识符
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"] ?? "")), // 通过应用程序配置中获取密钥字符串
            ClockSkew = TimeSpan.FromSeconds(30), // 允许的时间偏差
            RequireExpirationTime = true // JWT是否包含过期时间
        };

        options.UseSecurityTokenValidators = true;
        // 捕获并处理认证事件
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3VybmFtZSI6InRlc3QiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiVXNlciI6IjEiLCJQZXJtaXNzaW9uIjoiUGVybWlzc2lvbiIsIm5iZiI6MTcyNDcyNzk1OSwiZXhwIjoxNzI0ODE0MzU5LCJpc3MiOiJoZXlKdW4iLCJhdWQiOiJoZXlKdW4ifQ.vP9Nvz-88RpNXXH4UaGNIAd0XTSbhhfRCqqpwpN9Vyo") as JwtSecurityToken;
                if (jwtToken != null)
                {
                    Console.WriteLine(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
                }
                var exception = context.Exception;
            
                if (exception is SecurityTokenExpiredException)
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                else if (exception is SecurityTokenInvalidSignatureException)
                {
                    context.Response.Headers.Add("Token-Invalid-Signature", "true");
                }
                else if (exception is SecurityTokenInvalidAudienceException)
                {
                    context.Response.Headers.Add("Token-Invalid-Audience", "true");
                }
                else if (exception is SecurityTokenInvalidIssuerException)
                {
                    context.Response.Headers.Add("Token-Invalid-Issuer", "true");
                }
                else if (exception is SecurityTokenNoExpirationException)
                {
                    context.Response.Headers.Add("Token-No-Expiration", "true");
                }
                else
                {
                    // 添加日志记录
                    Console.WriteLine(exception.ToString(), "An unhandled exception occurred during authentication.");
                }
            
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3VybmFtZSI6InRlc3QiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiVXNlciI6IjEiLCJQZXJtaXNzaW9uIjoiUGVybWlzc2lvbiIsIm5iZiI6MTcyNDcyNzk1OSwiZXhwIjoxNzI0ODE0MzU5LCJpc3MiOiJoZXlKdW4iLCJhdWQiOiJoZXlKdW4ifQ.vP9Nvz-88RpNXXH4UaGNIAd0XTSbhhfRCqqpwpN9Vyo") as JwtSecurityToken;
                if (jwtToken != null)
                {
                    Console.WriteLine(jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
                }
                
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            
                if (claimsIdentity != null && !claimsIdentity.HasClaim(c => c.Type == "required_claim"))
                {
                    context.Fail("Unauthorized"); // 如果校验失败，终止请求
                }
            
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                // 自定义处理失败挑战的响应
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                // 获取异常信息
                var errorInfo = context.Error;
                var errorDescription = context.ErrorDescription;

                var result =
                    JsonConvert.SerializeObject(new { error = errorInfo, error_description = errorDescription });
                context.Response.WriteAsync(result);
                return Task.CompletedTask;
            }
        };
    });


// 配置Redis
var section = builder.Configuration.GetSection("Redis:Default");
var _connectionString = section.GetSection("Connection").Value ?? "";
var _instanceName = section.GetSection("InstanceName").Value ?? "";
int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
builder.Services.AddSingleton(new RedisUtils(_connectionString, _instanceName, _defaultDB));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


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
app.UseMiddleware<ResponseWrapperMiddleware>();

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