using AutoMapper;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos;

namespace MyBlog.Services.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>(); //User doner...
            CreateMap<User, UserUpdateDto>(); //UserUpdateDto doner...
            CreateMap<UserUpdateDto, User>(); //UserUpdateDto doner...
        }
    }
}
