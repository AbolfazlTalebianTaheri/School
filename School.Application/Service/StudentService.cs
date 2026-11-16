using AutoMapper;
using School.Application.DTOs;
using School.Application.Repository;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service
{
    public class StudentService(IStudentRepository studentRepository, IMapper mapper)
    {
        public async Task CreateAsync(StudentDto entity, CancellationToken cancellationToken = default)
        {
            var existing = await studentRepository.GetAllAsync(cancellationToken);
            if (existing.Any(s => s.StudentCode == entity.StudentCode))
                throw new School.Domain.Exceptions.DuplicateStudentException("این کد دانش‌آموزی قبلاً ثبت شده است.");
            if (existing.Any(s => s.NationalCode == entity.NationalCode))
                throw new School.Domain.Exceptions.DuplicateStudentException("این کد ملی قبلاً برای دانش‌آموز دیگری ثبت شده است.");
            await studentRepository.CreateAsync(mapper.Map<Student>(entity), cancellationToken);
            await studentRepository.SaveChangeAsync(cancellationToken);
        }

        public async Task DeleteAsync(ushort id, CancellationToken cancellationToken = default)
        {
            await studentRepository.DeleteAsync(id, cancellationToken);
            await studentRepository.SaveChangeAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
          mapper.Map<IReadOnlyList<StudentDto>>(await studentRepository.GetAllAsync(cancellationToken));
        public async Task<StudentDto?> GetByIdAsync(ushort id, CancellationToken cancellationToken = default) =>
            mapper.Map<StudentDto?>(await studentRepository.GetByIdAsync(id, cancellationToken));
        public async Task UpdateAsync(StudentDto entity, ushort id, CancellationToken cancellationToken = default)
        {
           await studentRepository.UpdateAsync(mapper.Map<Student>(entity), id, cancellationToken);
            await studentRepository.SaveChangeAsync(cancellationToken);
        }
    }
}
