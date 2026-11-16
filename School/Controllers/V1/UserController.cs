using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Constants;
using School.Api.Contracts;
using School.Api.Contracts.Lesson;
using School.Api.Contracts.User;
using School.Application.DTOs;
using School.Application.DTOs.User;
using School.Application.Service;
using School.Domain.Entitys;
using School.Domain.Enums;
using School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository;

namespace School.Api.Controllers.V1
{
    [Authorize(Roles = "Admin")]
    public class UserController(UserService userService, IMapper mapper) : BaseController
    {
        [HttpGet(UserUriConstants.GetAll)]
        public async Task<ApiResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var users = await userService.GetAllAsync(cancellationToken);
            var response = mapper.Map<IReadOnlyList<UserResponse>>(users); ;
            return ApiResult<IReadOnlyList<UserResponse>>.Succeeded(response);
        }
        [HttpGet(UserUriConstants.GetById)]
        public async Task<ApiResult<UserResponse?>> GetById(ushort id, CancellationToken cancellationToken = default)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);
            return mapper.Map<ApiResult<UserResponse?>>(user);
        }
        [HttpGet(UserUriConstants.GetAllTeachers)]
        public async Task<ApiResult<List<UserResponse>>> GetAllTeachers(CancellationToken cancellationToken = default)
        {
            var teachers = await userService.GetAllTeachersAsync(cancellationToken);
            return ApiResult<List<UserResponse>>.Succeeded(mapper.Map<List<UserResponse>>(teachers));
        }
        [HttpGet(UserUriConstants.GetBynationalCode)]
        public async Task<ApiResult<UserResponse?>> GetByNationalCode(string nationalCode, CancellationToken cancellationToken = default)
        {
            var user = userService.GetByNatioalCodeAsync(nationalCode, cancellationToken);
            return mapper.Map<UserResponse>(user);
        }
        [HttpGet(UserUriConstants.GetByRoleAndUserName)]
        public async Task<UserResponse?> GetByRoleAndUserName(UserRole role, string userName, CancellationToken cancellationToken = default)
        {
            var user = userService.GetByRoleAndUserNameAsync((UserRole)role, userName, cancellationToken);
            return mapper?.Map<UserResponse?>(user);
        }
        [HttpGet(UserUriConstants.UserNameExists)]
        public async Task<bool> UserNameExists(string userName, UserRole role, CancellationToken cancellationToken = default)
        {
            var isUser = await userService.UserNameExistsAsync(userName, role, cancellationToken);
            return isUser;
        }
        [HttpPost(UserUriConstants.Create)]
        public async Task<ApiResult> Create(CreateUserRequest userRequest, CancellationToken cancellationToken = default)
        {
            await userService.CreateAsync(mapper.Map<UserRsponseDto>(userRequest), cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpPost(UserUriConstants.AddLessonForTeacher)]
        public async Task<ApiResult> AddLessonForTeacher(ushort id, CreateLessonRequest request, CancellationToken cancellationToken)
        {
            await userService.AddLessonForTeacherAsync(id, mapper.Map<LessonDto>(request), cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpPut(UserUriConstants.Update)]
        public async Task<ApiResult> Update(UpdateUserRequest userRequest, ushort id, CancellationToken cancellationToken = default)
        {
            await userService.UpdateAsync(mapper.Map<UserRsponseDto>(userRequest), id, cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpDelete(UserUriConstants.Delete)]
        public async Task<ApiResult> Delete(ushort id, CancellationToken cancellationToken = default)
        {
            await userService.DeleteAsync(id, cancellationToken);
            return ApiResult.NoContent();
        }

    }
}
