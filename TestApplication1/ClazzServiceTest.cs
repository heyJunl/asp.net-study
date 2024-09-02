
using AutoFixture;
using AutoFixture.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using WebApplication1.Service;
using WebApplication1.Service.Impl;

namespace TestApplication1;

public class ClazzServiceTest
{
    private readonly IClazzService _clazz;
    private readonly IFixture _fixture;
    private readonly InfoContext _info;

    public ClazzServiceTest()
    {
        _fixture = new Fixture();

    }

    [Theory,AutoData]
    public async Task AddClazz(IClazzService _clazz)
    {
        var dto = _fixture.Create<ClazzAddDto>();
        await _clazz.Add(dto);
        var firstOrDefaultAsync = await _info.Clazz.Where(e=>e.Grade == dto.Grade).FirstOrDefaultAsync();
        Assert.NotNull(firstOrDefaultAsync);
        
    }
}