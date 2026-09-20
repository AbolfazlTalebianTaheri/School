using AutoMapper;
using School.Application.DTOs;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service
{
    public class UserService(IUserRepository userRepository, IMapper mapper)
    {
        public async Task CreateAsync(UserDto entity, CancellationToken cancellationToken)
        {
            await userRepository.CreateAsync(mapper.Map<User>(entity), cancellationToken = default);
            await userRepository.SaveChangeAsync(cancellationToken);
        }

        public Task CreateAsync(User entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(ushort id, CancellationToken cancellationToken = default)
        {
            await userRepository.DeleteAsync(id, cancellationToken);
            await userRepository.SaveChangeAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<UserDto>>(await userRepository.GetAllAsync(cancellationToken));

        public async Task<UserDto?> GetByIdAsync(ushort id, CancellationToken cancellationToken = default) => mapper.Map<UserDto?>(await userRepository.GetByIdAsync(id, cancellationToken));
        public async Task UpdateAsync(UserDto entity, ushort id, CancellationToken cancellationToken = default) => await userRepository.UpdateAsync(mapper.Map<User>(entity), id, cancellationToken);

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default) =>
            await userRepository.GetByPhoneNumberAsync(phoneNumber, cancellationToken);

        public async Task<User?> GetByRoleAndUserNameAsync(UserRole role, string userName, CancellationToken cancellationToken = default) =>
            await userRepository.GetByRoleAndUserNameAsync(role, userName, cancellationToken);
        public async Task<bool> UserNameExistsAsync(string userName, UserRole role, CancellationToken cancellationToken = default) =>
            await userRepository.UserNameExistsAsync(userName, role, cancellationToken);
    }
}
