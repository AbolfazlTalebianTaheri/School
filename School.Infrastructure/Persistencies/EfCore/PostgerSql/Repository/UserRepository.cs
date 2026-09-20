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
        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default) =>
            await _set.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);

        public async Task<User?> GetByRoleAndUserNameAsync(UserRole role, string userName, CancellationToken cancellationToken = default) => await _set.FirstOrDefaultAsync(x => x.UserRole == role && x.UserName == userName, cancellationToken);

        public async Task<bool> UserNameExistsAsync(string userName, UserRole userRole, CancellationToken cancellationToken = default) => await _set.AnyAsync(x => x.UserName == userName && x.UserRole == userRole, cancellationToken);
    }
}
