using AutoMapper;
using Microsoft.VisualBasic;
using School.Application.DTOs;
using School.Application.Repository;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service
{
    public class LessonService(ILessonRepository lessonRepository, IMapper mapper)
    {
        public async Task CreateAsync(LessonDto entity, CancellationToken cancellationToken = default)
        {
            await lessonRepository.CreateAsync(mapper.Map<Lesson>(entity), cancellationToken);
            await lessonRepository.SaveChangeAsync(cancellationToken);
        }
        public async Task DeleteAsync(ushort id, CancellationToken cancellationToken = default)
        {
            await lessonRepository.DeleteAsync(id, cancellationToken);
            await lessonRepository.SaveChangeAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<LessonDto>> GetAllAsync(CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<LessonDto>>(await lessonRepository.GetAllAsync(cancellationToken));
        public async Task<LessonDto?> GetByIdAsync(ushort id, CancellationToken cancellationToken = default) =>
            mapper.Map<LessonDto?>(await lessonRepository.GetByIdAsync(id, cancellationToken));
        public async Task UpdateAsync(LessonDto entity, ushort id, CancellationToken cancellationToken = default)
        {
            await lessonRepository.UpdateAsync(mapper.Map<Lesson>(entity), id, cancellationToken);
            await lessonRepository.SaveChangeAsync(cancellationToken);
        }
    }
}
