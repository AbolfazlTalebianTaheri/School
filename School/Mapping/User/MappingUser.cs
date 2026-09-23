using AutoMapper;
using School.Api.Contracts.User;
using School.Application.DTOs;

namespace School.Api.Mapping.User
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<UserDto, UserResponse>();


            CreateMap<UserResponse, UserDto>();
        }
    }
}
