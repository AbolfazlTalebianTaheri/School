using Microsoft.EntityFrameworkCore;
using School.Application.Repository;
using School.Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository
{
    public abstract class RepositoryBase<TEntity, TKey>(SchoolDbContext dbContext)
        : IRepository<TEntity, TKey> where TEntity : class, ISoftDeletable
    {
        public readonly SchoolDbContext _dbContext = dbContext;
        public readonly DbSet<TEntity> _set = dbContext.Set<TEntity>();
        public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            await _set.AddAsync(entity,cancellationToken);
        public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var existingEntity  = await _set.FindAsync(id,cancellationToken);
            if (existingEntity is null)
                return;
            existingEntity.IsDeleted = true;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>await _set.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) => await _set.FindAsync(id,cancellationToken);
        public async Task UpdateAsync(TEntity entity, TKey id, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _set.FindAsync(id,cancellationToken = default);
            if (existingEntity is null)
                return;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
        public async Task SaveChangeAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
