using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using School.Api.Constants;
using School.Api.Contracts;
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
            return mapper.Map<ApiResult<IReadOnlyList<StudentResponse>>>(students);
        }
        [HttpGet(StudentUriConstants.GetById)]
        public async Task<ApiResult<StudentResponse?>> GetById(ushort id, CancellationToken cancellationToken)
        {
            var student = await studentService.GetByIdAsync(id, cancellationToken);
            return mapper.Map<StudentResponse?>(student);
        }
        [HttpPost(StudentUriConstants.Create)]
        public async Task<ApiResult> Create(StudentResponse student, CancellationToken cancellationToken)
        {
            await studentService.CreateAsync(mapper.Map<StudentDto>(student), cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpDelete(StudentUriConstants.Delete)]
        public async Task<ApiResult> Delete(ushort id, CancellationToken cancellationToken)
        {
            await studentService.DeleteAsync(id, cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpPut(StudentUriConstants.Update)]
        public async Task<ApiResult> Update(StudentResponse student, ushort id, CancellationToken cancellationToken)
        {
            await studentService.UpdateAsync(mapper.Map<StudentDto>(student), id, cancellationToken);
            return ApiResult.NoContent();
        }

    }
}
