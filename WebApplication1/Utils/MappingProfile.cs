/*
 * @Author: Jun
 * @Description:
 */

using AutoMapper;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Vo;

namespace WebApplication1.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserPageVo>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        CreateMap<ClazzAddDto, Clazz>().ReverseMap();
        CreateMap<ClazzPageDto, Clazz>().ReverseMap();
        CreateMap<ClazzUpdateDto, Clazz>().ReverseMap();
        CreateMap<TeacherPageDto, Teacher>().ReverseMap();
        CreateMap<ClazzPageVo, Clazz>().ReverseMap();

        CreateMap<QueryResult, StudentClazzVo>();
        CreateMap<Clazz, StudentClazzVo>();
            // .ForMember(d => d.Grade, o => o.MapFrom(s => s.Grade))
            // .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            // .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
            // .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room)).ReverseMap();
            CreateMap<Student, StudentClazzVo>();
            // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            // .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex))
            // .ForMember(dest => dest.Birth, opt => opt.MapFrom(src => src.Birth))
            // .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            // .ForMember(dest => dest.Dept, opt => opt.MapFrom(src => src.Dept)).ReverseMap();


    }
}

public class QueryResult
{
    public Student Student{get;set;}
    public Clazz Clazz { get; set; }
}
