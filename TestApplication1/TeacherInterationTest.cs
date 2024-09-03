/*
 * @Author: Jun
 * @Description:
 */

using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.DbContexts;
using WebApplication1.Entity;
using WebApplication1.Service;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestApplication1;

public class TeacherInterationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IFixture _fixture = new Fixture();
    private readonly ITeacherService _teacher;
    private readonly InfoContext _info;
    
    public TeacherInterationTest(WebApplicationFactory<Program> factory, ITeacherService teacher, InfoContext context)
    {
        _factory = factory;
        _teacher = teacher;
        _info = context;
    }

    [Fact]
    public async Task Add()
    {
        // 添加 Add
        var client = _factory.CreateClient();
        var teacher = new Teacher() { Name = "Test01", Sex = 0, Phone = "12345678901", State = 0 };
        var json = JsonConvert.SerializeObject(teacher);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/teacher/add", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result =
            JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
        Assert.Equal("添加成功", result["value"]);
        
        
        // 查询 Query
        var teacherData = await _info.Teacher.Where(e=>e.Name == "Test01").FirstOrDefaultAsync();
        Assert.NotNull(teacherData);
        var uriBuilder = new UriBuilder("http://localhost:5246/api/Teacher/Query");
        uriBuilder.Query = $"id={teacherData.Id}";
        var response2 = await client.PostAsync(uriBuilder.Uri, null);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        result = String2Dict(response2);
        // Assert.Equal(result["value"]., );
        
    }

    public static Dictionary<string, string> String2Dict(HttpResponseMessage response)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
    }
    public static StringContent ConvertBodyContent<T>(T obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json");
    }
    
    public static StringContent ConvertParamContent<T>(T obj)
    {
        return new StringContent((JsonConvert.SerializeObject(obj)), System.Text.Encoding.UTF8, "application/json");
    }
}