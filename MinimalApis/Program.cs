using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApis;
using MinimalApis.EndPoints;
using MinimalApis.Exception;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddExceptionHandler<SystemExceptionHandle>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ResponseWrapperMiddleware>();
app.UseExceptionHandler();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapGet("/Test", User () => new User() { Name = "Jun", Email = "jun@gmail.com", Age = 18 }).WithSummary("测试类")
    .Produces<User>();

app.MapGet("/TestResult", IResultModel (int age) =>
    {
        List<User> users = [new User() { Name = "lee", Email = "lee@gmail.com", Age = 18 }];
        return users.FirstOrDefault(e => e.Age > age) is User user
            ? ResultModel.Success(user)
            : ResultModel.Failed();
    }).WithSummary("测试自定义IResultModel")
    .Produces<User>();

app.MapGet("/TestAutoWrapper",
        [EnableResponseWrapperAttribute] User () => new User() { Name = "lee", Email = "lee@gmail.com", Age = 18 })
    .WithSummary("测试自动包装").Produces<User>();

app.MapGet("/IResult/TestResult",
    IResult () => Results.Ok(new User() { Name = "lee", Email = "lee@gmail.com", Age = 18 }));
app.MapGet("/IResult/TestTypedResult",
    IResult () => TypedResults.Ok(new User() { Name = "Ruipeng", Email = "xxx@163.com", Age = 18 }));

app.MapGet("/IResult/ReturnMultipleType", Results<Ok<User>, NotFound> (int age) =>
{
    List<User> users = [new User() { Name = "lee", Email = "lee@gmail.com", Age = 18 }];
    return users.FirstOrDefault(e => e.Age > age) is User user ? TypedResults.Ok(user) : TypedResults.NotFound();
});

var options = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
app.MapGet("/IResult/CustomJsonConfig",
    () => TypedResults.Json(new User() { Name = "lee", Email = "lee@gmail.com", Age = 18 }));

app.MapGet("/IResult/ProblemDetail", () =>
{
    var problemDetail = new ProblemDetails()
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "内部错误"
    };
    return TypedResults.Problem(problemDetail);
});

app.MapGet("/CustomThrow", () =>
{
    throw new CustomException(StatusCodes.Status403Forbidden, "无权限");
}).WithOpenApi();

app.RegisterEndPoints();
app.Run();
