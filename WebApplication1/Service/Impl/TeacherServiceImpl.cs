/*
 * @Author: Jun
 * @Description:
 */

using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Common.Enums;
using WebApplication1.DbContexts;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Utils;
using WebApplication1.Vo;

namespace WebApplication1.Service.Impl;

public class TeacherServiceImpl : ITeacherService
{
    private readonly InfoContext _info;
    private readonly IMapper _mapper;

    public TeacherServiceImpl(InfoContext info, IMapper mapper)
    {
        this._info = info;
        this._mapper = mapper;
    }

    public async Task<ActionResult<string>> Add(Teacher teacher)
    {
        await _info.Teacher.AddAsync(teacher);
        await _info.SaveChangesAsync();
        return "添加成功";
    }

    public async Task<ActionResult<string>> Delete(string id)
    {
        var teacher = await _info.Teacher.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (teacher == null)
        {
            throw new Exception("教师Id不存在");
        }

        var clazzNumber = await _info.Clazz.FirstOrDefaultAsync(e => e.TeacherId == id);
        if (clazzNumber != null)
        {
            throw new Exception("请完成老师交接工作后操作（对应班级更换老师）");
        }

        teacher.State = StateType.DEACTIVATE.GetHashCode();
        _info.Update(teacher);
        await _info.SaveChangesAsync();
        return "删除成功";
    }

    public async Task<ActionResult<string>> Update(UpdateTeacherDto teacher)
    {
        var query = await _info.Teacher.FirstOrDefaultAsync(e=>e.Id == teacher.Id);
        _mapper.Map(teacher, query);
        await _info.SaveChangesAsync();
        return "更新成功";
    }

    public async Task<ActionResult<PaginatedResponse<Teacher>>> QueryTeacherPage(PageTeacherDto dto)
    {
        var teacher = _mapper.Map<Teacher>(dto);
        var total = await _info.Teacher.CountAsync();
        var page = new PageParam(dto.PageNo.Value, dto.PageSize.Value, total);
        var wrapper = _info.Teacher.AsQueryable();
        if (!string.IsNullOrWhiteSpace(teacher.Id))
        {
            wrapper = wrapper.Where(e => e.Id == teacher.Id);
        }

        if (!string.IsNullOrWhiteSpace(teacher.Name))
        {
            wrapper = wrapper.Where(e => teacher.Name.Contains(e.Name));
        }

        if (teacher.State != null)
        {
            wrapper = wrapper.Where(e => e.State == teacher.State);
        }

        var listAsync = await wrapper.Skip((page.PageNo.Value - 1) * page.PageSize.Value).Take(page.PageSize.Value)
            .OrderBy(e => e.CreateTime).ToListAsync();
        return new PaginatedResponse<Teacher>(listAsync, page);
    }

    public async Task<ActionResult<Teacher>> Query(string id)
    {
        var teacher = await _info.Teacher.FirstOrDefaultAsync(e => e.Id == id);
        if (teacher == null)
        {
            throw new Exception("教师Id不存在");
        }

        return teacher;
    }

    public async Task<ActionResult<string>> EntrustWork(string outId, string inId)
    {
        var check = _info.Teacher.Where(e=>e.Id==outId || e.Id == inId).ToListAsync();
        if (check.Result.Count != 2)
        {
            throw new Exception("请确认双方信息是否正确");
        }
        
        var clazzInfo = _info.Clazz.Where(e => e.TeacherId == outId).ToListAsync().Result;
        clazzInfo.ForEach(e=> e.TeacherId = inId);
        await _info.SaveChangesAsync();
        return "交接成功";
    }

    public async Task<ActionResult<List<StudentClazzVo>>> ChargedWork(string id)
    {
        var clazzs = await _info.Clazz.Where(e=> e.TeacherId == id).Select(e => e.TeacherId).ToListAsync();
        
        var result = await (from a in _info.Student
                join b in _info.Clazz on a.ClassId equals b.Id
                where clazzs.Contains(b.TeacherId) && b.State == StateType.ACTIVATE.GetHashCode()
                select new StudentClazzVo() 
                {
                    Name = a.Name, Number = b.Number, Sex = a.Sex, Grade = b.Grade, Year = b.Year,
                    Room = b.Room, Dept = a.Dept, Address = a.Address, Birth = a.Birth
                })
            .ToListAsync();
        return result;
        
        // var result = await (from a in _info.Student
        //         join b in _info.Clazz on a.ClassId equals b.Id
        //         where b.TeacherId == "1828961840986722304" && b.State == StateType.ACTIVATE.GetHashCode()
        //         select new QueryResult{Clazz = b, Student = a})
        //     .ProjectTo<StudentClazzVo>(_mapper.ConfigurationProvider)
        //     .ToListAsync();
        // return null;
    }
}