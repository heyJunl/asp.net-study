/*
 * @Author: Jun
 * @Description:
 */

using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common.Enums;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Service.Impl;

public class ClazzServiceImpl: IClazzService
{
    private readonly InfoContext _info;
    private readonly IMapper _mapper;

    public ClazzServiceImpl(InfoContext info, IMapper mapper)
    {
        this._info = info;
        this._mapper = mapper;
    }

    public async Task<ActionResult<string>> Add(ClazzAddDto clazz)
    {
        var result = _mapper.Map<Clazz>(clazz);
        await _info.Clazz.AddAsync(result);
        await _info.SaveChangesAsync();
        return "添加成功";
    }

    public async Task<ActionResult<string>> Delete(string id)
    {
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == id);
        if (clazz.Total > 0)
        {
            throw new Exception("该班级有学生，无法删除");
        }
        clazz.State = StateType.DEACTIVATE.GetHashCode();
        _info.SaveChangesAsync();
        return "删除成功";
    }

    public async Task<ActionResult<PaginatedResponse<ClazzPageVo>>> Page(ClazzPageDto dto)
    {
        var totalCount = await _info.Clazz.CountAsync();
        var page = new PageParam(dto.PageNo.Value, dto.PageSize.Value, totalCount);
        // var wrapper = _info.Clazz.AsQueryable();
        IQueryable<Clazz> wrapper = WrapperConstruct(dto);
        var clazzList = await wrapper.Skip((page.PageNo.Value - 1) * page.PageSize.Value).Take(page.PageSize.Value)
            .OrderBy(e => e.CreateTime).ToListAsync();
        var result = _mapper.Map<List<ClazzPageVo>>(clazzList);
        return new PaginatedResponse<ClazzPageVo>(result, page);
    }

    public async Task<ActionResult<string>> Update(ClazzUpdateDto dto)
    {
        var clazz = _mapper.Map<Clazz>(dto);
        var x = await _info.Clazz.AsNoTracking().FirstOrDefaultAsync(e => e.Id == clazz.Id);
        if (x ==null)
        {
            throw new Exception("班级不存在");
        }
        clazz.Total = x.Total;
        _info.Update(clazz);
        await _info.SaveChangesAsync();
        return "更新成功";

    }

    public async Task<ActionResult<Clazz>> Query(string id)
    {
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == id);
        if (clazz == null)
        {
            throw new Exception("班级不存在");
        }

        return clazz;
    }
    
    
    
    public IQueryable<Clazz> WrapperConstruct<T>(T input)
    {
        var clazz = _mapper.Map<Clazz>(input);
        var wrapper = _info.Clazz.AsQueryable();
        if (!string.IsNullOrWhiteSpace(clazz.Grade))
        {
            wrapper = wrapper.Where(e => e.Grade == clazz.Grade);
        }

        if (!String.IsNullOrWhiteSpace(clazz.Id))
        {
            wrapper = wrapper.Where(e => e.Id == clazz.Id);
        }
        if (!String.IsNullOrWhiteSpace(clazz.Number))
        {
            wrapper = wrapper.Where(e => e.Number == clazz.Number);
        }
        if (!String.IsNullOrWhiteSpace(clazz.Year))
        {
            wrapper = wrapper.Where(e => e.Year == clazz.Year);
        }
        if (!String.IsNullOrWhiteSpace(clazz.Room))
        {
            wrapper = wrapper.Where(e => e.Room == clazz.Room);
        }
        if (!String.IsNullOrWhiteSpace(clazz.TeacherId))
        {
            wrapper = wrapper.Where(e => e.TeacherId == clazz.TeacherId);
        }
        if (clazz.Sub != null)
        {
            wrapper = wrapper.Where(e => e.Sub == clazz.Sub);
        }

        return wrapper;
    }
}