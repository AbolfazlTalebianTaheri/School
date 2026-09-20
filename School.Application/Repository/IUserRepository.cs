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

        Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

        Task<bool> UserNameExistsAsync(string userName, UserRole role, CancellationToken cancellationToken = default);
    }
}
