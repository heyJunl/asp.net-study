/*
 * @Author: Jun
 * @Description:
 */

using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApis.Exception;
using WebApplication1.Common;
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

    public async Task<ActionResult<string>> Add(AddClazzDto addClazz)
    {
        var result = _mapper.Map<Clazz>(addClazz);
        await _info.Clazz.AddAsync(result);
        await _info.SaveChangesAsync();
        return "添加成功";
    }

    public async Task<ActionResult<string>> Delete(string id)
    {
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == id);
        if (clazz?.Total > 0)
        {
            throw new CustomException(StatusCodes.Status400BadRequest.GetHashCode(), "该班级有学生，无法删除");
        }
        clazz.State = StateType.DEACTIVATE.GetHashCode();
        await _info.SaveChangesAsync();
        return "删除成功";
    }

    public async Task<ActionResult<PaginatedResponse<PageClazzVo>>> Page(PageClazzDto dto)
    {
        var totalCount = await _info.Clazz.CountAsync();
        var page = new PageParam(dto.PageNo.Value, dto.PageSize.Value, totalCount);
        // var wrapper = _info.Clazz.AsQueryable();
        IQueryable<Clazz> wrapper = WrapperConstruct(dto);
        var clazzList = await wrapper.Skip((page.PageNo.Value - 1) * page.PageSize.Value).Take(page.PageSize.Value)
            .OrderBy(e => e.CreateTime).ToListAsync();
        var result = _mapper.Map<List<PageClazzVo>>(clazzList);
        return new PaginatedResponse<PageClazzVo>(result, page);
    }

    public async Task<ActionResult<string>> Update(AddClazzUpdateDto dto)
    {
        var resource = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == dto.Id);
        if (resource ==null)
        {
            throw new CustomException(StatusCodes.Status400BadRequest.GetHashCode(), "班级不存在");
        }

        _mapper.Map(dto, resource);
        await _info.SaveChangesAsync();
        return "更新成功";

    }

    public async Task<ActionResult<Clazz>> Query(string id)
    {
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == id);
        if (clazz == null)
        {
            throw new CustomException(StatusCodes.Status400BadRequest.GetHashCode(), "班级不存在");
        }

        return clazz;
    }

    public async Task<ActionResult<List<GradeStudentVo>>> QueryClazzStudent(string clazzId)
    {
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e=> e.Id == clazzId);
        if (clazz == null)
        {
            throw new CustomException(StatusCodes.Status400BadRequest.GetHashCode(), "班级不存在");
        }

        var teacher = await _info.Teacher.Where(e => e.Id == clazz.TeacherId).FirstOrDefaultAsync();
        if (teacher == null)
        {
            throw new CustomException(StatusCodes.Status400BadRequest.GetHashCode(), "教师信息异常");
        }
        var studentList = await _info.Student.Where(e => e.ClassId == clazzId).ToListAsync();
        var result = new List<GradeStudentVo>();
        studentList.ForEach(e =>
        {
            result.Add(new GradeStudentVo()
            {
                Grade = clazz.Grade, Total = clazz.Total, Sub = clazz.Sub, TeacherName = teacher.Name, StudentName = e.Name
            });
        });
        return result;
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