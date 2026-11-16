using AutoMapper;
using Microsoft.AspNetCore.Identity;
using School.Application.DTOs;
using School.Application.DTOs.User;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using School.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service
{
    public class UserService(IUserRepository userRepository, IStudentRepository studentRepository, IPasswordHasher<User> passwordHasher, IMapper mapper)
    {
        public async Task CreateAsync(UserRsponseDto entity, CancellationToken cancellationToken)
        {
            if (entity.UserRole == UserRole.Parent)
            {
                // والد با کد ملی دانش‌آموز وارد می‌شود؛ پس باید دانش‌آموز وصل شود
                if (entity.StudentId is not ushort studentId)
                    throw new EntityNotFoundException(nameof(Student), "student");

                var student = await studentRepository.GetByIdAsync(studentId, cancellationToken);
                if (student is null || student.IsDeleted)
                    throw new EntityNotFoundException(nameof(Student), studentId);

                var parent = mapper.Map<User>(entity);
                parent.AddStudent(student);
                await userRepository.CreateAsync(parent, cancellationToken);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(entity.UserName) &&
                    await userRepository.UserNameExistsAsync(entity.UserName, entity.UserRole, cancellationToken))
                    throw new DuplicateUserNameException();

                if (entity.Password is not null)
                    entity.Password = passwordHasher.HashPassword(mapper.Map<User>(entity), entity.Password);

                await userRepository.CreateAsync(mapper.Map<User>(entity), cancellationToken);
            }

            await userRepository.SaveChangeAsync(cancellationToken);
        }
        public async Task AddLessonForTeacherAsync(ushort id, LessonDto lessonDto, CancellationToken cancellationToken)
        {
            await userRepository.AddLessonForTeacherAsync(id, mapper.Map<Lesson>(lessonDto), cancellationToken);
            await userRepository.SaveChangeAsync(cancellationToken);
        }
        public async Task DeleteAsync(ushort id, CancellationToken cancellationToken = default)
        {
            await userRepository.DeleteAsync(id, cancellationToken);
            await userRepository.SaveChangeAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<UserRsponseDto>> GetAllAsync(CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<UserRsponseDto>>(await userRepository.GetAllWithStudentsAsync(cancellationToken));
        public async Task<UserRsponseDto?> GetByIdAsync(ushort id, CancellationToken cancellationToken = default) => mapper.Map<UserRsponseDto?>(await userRepository.GetByIdAsync(id, cancellationToken));
        public async Task UpdateAsync(UserRsponseDto entity, ushort id, CancellationToken cancellationToken = default) => await userRepository.UpdateAsync(mapper.Map<User>(entity), id, cancellationToken);
        public async Task<User?> GetByNatioalCodeAsync(string nationalCode, CancellationToken cancellationToken = default) =>
            await userRepository.GetByNationalCodeAsync(nationalCode, cancellationToken);
        public async Task<User?> GetByRoleAndUserNameAsync(UserRole role, string userName, CancellationToken cancellationToken = default) =>
            await userRepository.GetByRoleAndUserNameAsync(role, userName, cancellationToken);
        public async Task<bool> UserNameExistsAsync(string userName, UserRole role, CancellationToken cancellationToken = default) =>
            await userRepository.UserNameExistsAsync(userName, role, cancellationToken);
        public async Task<List<UserRsponseDto>> GetAllTeachersAsync(CancellationToken cancellationToken = default) =>
            mapper.Map<List<UserRsponseDto>>(await userRepository.GetAllTeachersAsync(cancellationToken));
        public async Task<UserRsponseDto?> GetByIdTeacherAsync(ushort id, CancellationToken cancellationToken = default) =>
            mapper.Map<UserRsponseDto?>(await userRepository.GetByIdTeacherAsync(id, cancellationToken));
    }
}
