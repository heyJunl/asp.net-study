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



    }
}