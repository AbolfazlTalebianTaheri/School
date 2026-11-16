using School.Application.Repository;
using School.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service
{
    public sealed class ExceptionLogService(IExceptionLogRepository repository)
    {
        public Task LogAsync(Exception exception, int statusCode, string? requestPath = null, string? requestMethod = null, CancellationToken cancellationToken = default)
        {
            var log = ExceptionLog.CreateByException(
                exception,
                statusCode,
                requestPath,
                requestMethod);
            return repository.AddAsync(log, cancellationToken);
        }
        public Task<ExceptionLog?> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default) =>
             repository.GetByIdAsync(id, cancellationToken);
        public Task<IReadOnlyList<ExceptionLog>> GetAllAsync(
            CancellationToken cancellationToken = default) =>
             repository.GetAllAsync(cancellationToken);
        public Task<IReadOnlyList<ExceptionLog>> GetRecentAsync(int count, CancellationToken cancellationToken = default) =>
            repository.GetRecentAsync(count, cancellationToken);
    }
}
