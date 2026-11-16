using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Constants;
using School.Api.Contracts;
using School.Api.Contracts.Lesson;
using School.Api.Contracts.Student;
using School.Application.DTOs;
using School.Application.Service;
using School.Domain.Entitys;
using School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository;

namespace School.Api.Controllers.V1
{
    [ApiVersion(1.0)]
    public class StudentController(StudentService studentService, IMapper mapper) : BaseController
    {
        [HttpGet(StudentUriConstants.GetAll)]
        public async Task<ApiResult<IReadOnlyList<StudentResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var students = await studentService.GetAllAsync(cancellationToken);
            IReadOnlyList<StudentResponse> response = mapper.Map<IReadOnlyList<StudentResponse>>(students);

            // والدین فقط دانش‌آموزان وصل‌شده به حساب خودشان را می‌بینند
            if (IsParent)
            {
                var own = OwnStudentIds;
                response = response.Where(s => own.Contains(s.Id)).ToList();
            }

            return ApiResult<IReadOnlyList<StudentResponse>>.Succeeded(response);
        }

        [HttpGet(StudentUriConstants.GetById)]
        public async Task<ApiResult<StudentResponse?>> GetById(ushort id, CancellationToken cancellationToken)
        {
            if (IsParent && !OwnStudentIds.Contains(id))
                return ApiResult<StudentResponse?>.Failed(
                    new { message = "شما به اطلاعات این دانش‌آموز دسترسی ندارید." },
                    StatusCodes.Status403Forbidden);

            var student = await studentService.GetByIdAsync(id, cancellationToken);
            return mapper.Map<StudentResponse?>(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(StudentUriConstants.Create)]
        public async Task<ApiResult> Create(CreateStudentRequest student, CancellationToken cancellationToken)
        {
            await studentService.CreateAsync(mapper.Map<StudentDto>(student), cancellationToken);
            return ApiResult.NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(StudentUriConstants.Delete)]
        public async Task<ApiResult> Delete(ushort id, CancellationToken cancellationToken)
        {
            await studentService.DeleteAsync(id, cancellationToken);
            return ApiResult.NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(StudentUriConstants.Update)]
        public async Task<ApiResult> Update(StudentResponse student, ushort id, CancellationToken cancellationToken)
        {
            await studentService.UpdateAsync(mapper.Map<StudentDto>(student), id, cancellationToken);
            return ApiResult.NoContent();
        }
    }
}
