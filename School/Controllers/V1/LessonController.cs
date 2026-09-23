using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using School.Api.Constants;
using School.Api.Contracts;
using School.Api.Contracts.Lesson;
using School.Application.DTOs;
using School.Application.Service;
using School.Domain.Entitys;

namespace School.Api.Controllers.V1
{
    public class LessonController(LessonService lessonService, IMapper mapper) : BaseController
    {
        //[HttpGet(LessonUriContracts.GetAll)]
        //public async Task<ApiResult<IReadOnlyList<LessonResponse>>> GetAll(CancellationToken cancellationToken)
        //{
        //    var lessons = await lessonService.GetAllAsync(cancellationToken);
        //    return mapper.Map<ApiResult<IReadOnlyList<LessonResponse>>>(lessons);
        //}
        [HttpGet(LessonUriContracts.GetAll)]
        public async Task<ApiResult<IReadOnlyList<LessonResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var lessons = await lessonService.GetAllAsync(cancellationToken);
            var response =  mapper.Map<IReadOnlyList<LessonResponse>>(lessons);
            return ApiResult<IReadOnlyList<LessonResponse>>.Succeeded(response);
        }
        [HttpGet(LessonUriContracts.GetById)]
        public async Task<ApiResult<LessonResponse?>> GetById(ushort id, CancellationToken cancellationToken)
        {
            var lesson = await lessonService.GetByIdAsync(id, cancellationToken);
            return mapper.Map<LessonResponse>(lesson);
        }
        [HttpPost(LessonUriContracts.Create)]
        public async Task<ApiResult> Create(LessonResponse response, CancellationToken cancellationToken)
        {
            await lessonService.CreateAsync(mapper.Map<LessonDto>(response), cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpPut(LessonUriContracts.Update)]
        public async Task<ApiResult> Update(LessonResponse response, ushort id, CancellationToken cancellationToken)
        {
            await lessonService.UpdateAsync(mapper.Map<LessonDto>(response), id, cancellationToken);
            return ApiResult.NoContent();
        }
        [HttpDelete(LessonUriContracts.Delete)]
        public async Task<ApiResult> Delete(ushort id, CancellationToken cancellationToken)
        {
            await lessonService.DeleteAsync(id, cancellationToken);
            return ApiResult.NoContent();
        }
    }
}
