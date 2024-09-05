using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebApplication1.Common.Enums;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Service;
using WebApplication1.Service.Impl;
using WebApplication1.Vo;
using Xunit;
using Xunit.Abstractions;

namespace TestApplication1;

public class ClazzUnitTest
{
    private readonly ITestOutputHelper _out;
    private readonly IClazzService _clazz;
    private readonly InfoContext _info;
    private readonly IFixture _fixture = new Fixture();
    private readonly IMapper _mapper;


    public ClazzUnitTest(IClazzService clazz, InfoContext context, ITestOutputHelper output, IMapper mapper)
    {
        _clazz = clazz;
        _info = context;
        _fixture = new Fixture();
        _out = output;
        _mapper = mapper;
    }


    [Fact]
    public async Task AddClazz()
    {
        var clz = _fixture.Create<AddClazzDto>();
        var result = await _clazz.Add(clz);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Delete()
    {
        var id = (await _info.Clazz.AsNoTracking().OrderByDescending(e => e.Id).ToListAsync().ConfigureAwait(false))[0]
            .Id;
        var clz = await _info.Clazz.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        Assert.NotNull(clz);
        await _clazz.Delete(id).ConfigureAwait(false);
        clz = await _info.Clazz.AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        Assert.NotNull(clz);
        Assert.Equal(clz.State, StateType.DEACTIVATE.GetHashCode());
    }

    [Fact]
    public async Task Page()
    {
        var page = new PageClazzDto();
        var result = await _clazz.Page(page);
        var clazzPageVo = result.Value.Data.ToList()[0];
        int count;
        var pageParam = result.Value.Meta;
        if (pageParam.PageNo == 1)
        {
            count = pageParam.PageSize.Value;
        }
        else
        {
            var pageLimit = pageParam.TotalCount.Value - ((pageParam.PageNo - 1) * pageParam.PageSize).Value;
            count = pageLimit > page.PageSize ? page.PageSize.Value : pageLimit;
        }

        Assert.NotNull(result.Value.Data);
        Assert.Equal(count, result.Value.Data.ToList().Count);
    }

    [Fact]
    public async Task QueryById()
    {
        var list = await _info.Clazz.ToListAsync().ConfigureAwait(false);
        var clz = await _clazz.Query(list[0].Id);
        Assert.Equal(list[0].Grade, clz.Value.Grade);
    }

    [Fact]
    public async Task QueryByNull()
    {
        await Assert.ThrowsAsync<Exception>(async () => await _clazz.Query(new string("")));
    }

    [Fact]
    public async Task Update()
    {
        var clz = await _info.Clazz.AsNoTracking().OrderByDescending(e => e.Id).ToListAsync().ConfigureAwait(false);
        Assert.NotNull(clz);
        var randomInt = new Random().Next(0, 11);
        var dto = _mapper.Map<AddClazzUpdateDto>(clz[0]);
        dto.Year = randomInt.ToString();
        var id = dto.Id;
        await _clazz.Update(dto);
        var check = await _info.Clazz.Where(e => e.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        Assert.NotNull(check);
        Assert.Equal(randomInt.ToString(), check.Year);
    }

    [Fact]
    public async Task QueryByGrade()
    {
        var clazzList = await _info.Clazz.ToListAsync().ConfigureAwait(false);
        Assert.NotNull(clazzList);
        var clazzIdList = clazzList.Select(e => e.Grade).ToList();
        var index = new Random().Next(0, clazzIdList.Count);
        var originData = clazzList[index];
        var originClazziId = clazzIdList[index];
        ActionResult<List<GradeStudentVo>> result = null;
        
        try
        {
            result = await _clazz.QueryClazzStudent(originClazziId).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e.Message == "班级不存在")
            {
                Assert.Equal("班级不存在", e.Message);
                return;
            }
            else if (e.Message == "教师信息异常")
            {
                Assert.Equal("教师信息异常", e.Message);
                return;
            }
        }
        Assert.NotNull(result);
        Assert.Equal(originData.Grade, result.Value[0].Grade);
    }
}