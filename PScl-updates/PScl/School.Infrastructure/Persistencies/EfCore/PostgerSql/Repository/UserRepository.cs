using Microsoft.EntityFrameworkCore;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository
{
    public class UserRepository(SchoolDbContext dbContext) : RepositoryBase<User, ushort>(dbContext), IUserRepository
    {
        public async Task<User?> GetByRoleAndNationalCodeAsync(UserRole role, string nationalCode, CancellationToken cancellationToken = default) =>
            await _set.FirstOrDefaultAsync(
                x => !x.IsDeleted && x.UserRole == role && x.Students.Any(s => s.NationalCode == nationalCode && !s.IsDeleted),
                cancellationToken);
        public async Task<User?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default) =>
            await _set.FirstOrDefaultAsync(
                x => !x.IsDeleted && x.Students.Any(s => s.NationalCode == nationalCode && !s.IsDeleted),
                cancellationToken);
        public async Task<List<Student>> GetStudentsAsync(ushort userId, CancellationToken cancellationToken = default) =>
            await _set.Where(x => x.Id == userId)
                .SelectMany(x => x.Students)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        public async Task<IReadOnlyList<User>> GetAllWithStudentsAsync(CancellationToken cancellationToken = default) =>
            await _set.Where(x => !x.IsDeleted).Include(x => x.Students).AsNoTracking().ToListAsync(cancellationToken);
        public async Task<User?> GetByRoleAndUserNameAsync(UserRole role, string userName, CancellationToken cancellationToken = default) => await _set.FirstOrDefaultAsync(x => x.UserRole == role && x.UserName == userName, cancellationToken);

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default) =>
            await _set.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
        public async Task<bool> UserNameExistsAsync(string userName, UserRole userRole, CancellationToken cancellationToken = default) => await _set.AnyAsync(x => x.UserName == userName && x.UserRole == userRole, cancellationToken);
        public async Task AddLessonForTeacherAsync(ushort id, Lesson lesson, CancellationToken cancellationToken = default)
        {
            var teacher = await _set.FirstOrDefaultAsync(x => x.Id == id && x.UserRole == UserRole.Teacher, cancellationToken);
            if (teacher is null)
                return;
            teacher.AddLesson(lesson);
        }
        public async Task<List<User>> GetAllTeachersAsync(CancellationToken cancellationToken = default) => await _set.Where(x => x.UserRole == UserRole.Teacher).ToListAsync(cancellationToken);
        public async Task<User?> GetByIdTeacherAsync(ushort id, CancellationToken cancellationToken = default) =>
            await _set.FirstOrDefaultAsync(x => x.UserRole == UserRole.Teacher && x.Id == id, cancellationToken);
    }
}
