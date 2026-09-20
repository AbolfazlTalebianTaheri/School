using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Repository
{
    public interface IRepository<TEntity,TKey> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TKey id,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        Task UpdateAsync (TEntity entity, TKey id, CancellationToken cancellationToken = default);
        Task SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
