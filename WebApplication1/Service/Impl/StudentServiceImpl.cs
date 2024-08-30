/*
 * @Author: Jun
 * @Description:
 */

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;
using WebApplication1.Entity;

namespace WebApplication1.Service.Impl;

public class StudentServiceImpl : IStudentService
{
    private readonly InfoContext _info;

    public StudentServiceImpl(InfoContext info)
    {
        this._info = info;
    }


    public async Task<ActionResult<string>> Add(Student student)
    {
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == student.ClassId);
        if (clazz == null)
        {
            throw new Exception("班级信息异常");
        }
        _info.Student.Add(student);
        clazz.Total++;
        
        await _info.SaveChangesAsync();
        return "添加成功";
    }

    public async Task<ActionResult<string>> Update(Student student)
    {
        var originData = await _info.Student.AsNoTracking().FirstOrDefaultAsync(e=>e.Id == student.Id);
        if (originData == null || string.IsNullOrWhiteSpace(student.Id))
        {
            throw new Exception("学生信息异常");
        }
        var originClazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == originData.ClassId);
        if (originClazz == null || string.IsNullOrWhiteSpace(originData.ClassId))
        {
            throw new Exception("班级信息异常");
        }

        if (string.IsNullOrWhiteSpace(student.Id) || originData == null)
        {
            return "学生信息异常";
        }

        if (student.ClassId != originData.ClassId)
        {
            originClazz.Total--;
            var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id == student.ClassId);
            clazz.Total++;
        }
        _info.Student.Update(student);
        // _info.Entry(student).State = EntityState.Modified;
        await _info.SaveChangesAsync();
        

        return "更新成功！";
    }

    public async Task<ActionResult<string>> Delete(string id)
    {
        var student = await _info.Student.FirstOrDefaultAsync(e=>e.Id==id);
        if (student == null)
        {
            throw new Exception("学生信息异常！");
        }
        var clazz = await _info.Clazz.FirstOrDefaultAsync(e => e.Id==student.ClassId);
        clazz.Total--;
        // _info.Entry(student).State = EntityState.Deleted;
        _info.Student.Remove(student);
     
        await _info.SaveChangesAsync();
   


        return "删除成功！";
    }

    public async Task<ActionResult<IEnumerable<Student>>> Query()
    {
        return await _info.Student.ToListAsync();
        // return _info.Student.ToList();
    }

    public async Task<ActionResult<Student>> QueryById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException("id不能为空！");
        }

        return await _info.Student.FindAsync(id);
    }

    public async Task<ActionResult<PaginatedResponse<Student>>> Page(Student student)
    {
        if (student.PageParam is not null)
        {
            var totalCount = await _info.Student.CountAsync();
            var page =
                new PageParam(student.PageParam.PageNo.Value, student.PageParam.PageSize.Value, totalCount);

            var query = _info.Student.AsQueryable();
            if (!string.IsNullOrWhiteSpace(student.Name))
            {
                query = query.Where(e => e.Name.Contains(student.Name));
            }

            if (student.Sex.HasValue)
            {
                query = query.Where(e => e.Sex == student.Sex);
            }

            // query.Where(e => string.IsNullOrWhiteSpace(student.Name) || e.Name.Contains(student.Name));
            var students = await query.Skip((page.PageNo.Value - 1) * page.PageSize.Value)
                .Take(page.PageSize.Value)
                .ToListAsync();

            return new PaginatedResponse<Student>(students, page);
        }

        throw new ArgumentNullException("请输入分页参数！");
    }

    public async Task<ActionResult<List<Student>>> QueryClassmate(string id)
    {
        var student = _info.Student.FirstOrDefaultAsync(e => e.Id == id);
        if (student.Result == null)
        {
            throw new Exception("学生不存在！");
        }
        var list = _info.Student.Where(e => e.ClassId == student.Result.ClassId).ToListAsync();
        return list.Result;

    }
}