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
        _info.Student.Add(student);
        await _info.SaveChangesAsync();
        return "添加成功";
    }

    public async Task<ActionResult<string>> Update(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Id))
        {
            return "id不能为空！";
        }

        _info.Student.Update(student);

        // _info.Entry(student).State = EntityState.Modified;

        try
        {
            await _info.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            return e.Data.ToString();
        }

        return "更新成功！";
    }

    public async Task<ActionResult<string>> Delete(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Id))
        {
            return "id不能为空！";
        }

        // _info.Entry(student).State = EntityState.Deleted;
        _info.Student.Remove(student);
        try
        {
            await _info.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return e.Data.ToString();
        }

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