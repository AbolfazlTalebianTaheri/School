using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Repository
{
    public interface IUserRepository : IRepository<User, ushort>
    {
        Task<User?> GetByRoleAndUserNameAsync(UserRole role, string userName, CancellationToken cancellationToken = default);
        /// <summary>حساب والدی که به دانش‌آموزی با این کد ملی وصل است.</summary>
        Task<User?> GetByRoleAndNationalCodeAsync(UserRole role, string nationalCode, CancellationToken cancellationToken = default);
        Task<User?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default);
        Task<bool> UserNameExistsAsync(string userName, UserRole role, CancellationToken cancellationToken = default);
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task AddLessonForTeacherAsync(ushort id, Lesson lesson,CancellationToken cancellationToken);
        Task<List<User>> GetAllTeachersAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByIdTeacherAsync(ushort id,CancellationToken cancellationToken = default);
        /// <summary>دانش‌آموزان (حذف‌نشده) وصل‌شده به یک کاربر والد.</summary>
        Task<List<Student>> GetStudentsAsync(ushort userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetAllWithStudentsAsync(CancellationToken cancellationToken = default);
    }
}
