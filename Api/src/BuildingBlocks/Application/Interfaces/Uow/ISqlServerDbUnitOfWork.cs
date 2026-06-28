using System.Data;

namespace BuildingBlocks.Application.Interfaces.Uow;

public interface ISqlServerDbUnitOfWork : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }

    bool HasActiveTransaction { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task CreateSavepointAsync(string name, CancellationToken cancellationToken = default);

    Task RollbackToSavepointAsync(string name, CancellationToken cancellationToken = default);

    string Offset();
}