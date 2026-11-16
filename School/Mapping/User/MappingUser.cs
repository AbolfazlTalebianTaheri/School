using AutoMapper;
using School.Api.Contracts.User;
using School.Application.DTOs.User;

namespace School.Api.Mapping.User
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<UserRsponseDto, UserResponse>();
            CreateMap<UserResponse, UserRsponseDto>();
            CreateMap<UserRsponseDto, UserResponse>();
            CreateMap<UserResponse, UserRsponseDto>();
            CreateMap<CreateUserRequestDto, CreateUserRequest>();
            CreateMap<CreateUserRequest, CreateUserRequestDto>();
            CreateMap<CreateUserRequest, UserRsponseDto>();
            CreateMap<UserRsponseDto, UpdateUserRequest>();
            CreateMap<UpdateUserRequest, UserRsponseDto>();
        }
    }
}
