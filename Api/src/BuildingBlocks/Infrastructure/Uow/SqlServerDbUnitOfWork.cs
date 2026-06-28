using System.Data;
using BuildingBlocks.Application.Interfaces.Uow;
using Dapper;

namespace BuildingBlocks.Infrastructure.Uow;

public class SqlServerDbUnitOfWork : ISqlServerDbUnitOfWork
{
    public IDbConnection Connection { get; }
    private IDbTransaction? _transaction;

    public IDbTransaction? Transaction => _transaction;
    public bool HasActiveTransaction => _transaction != null;


    private readonly ISqlServerDbChangesWriter _dbChangesWriter;
    public SqlServerDbUnitOfWork(IDbConnection connection, ISqlServerDbChangesWriter dbChangesWriter)
    {
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        _dbChangesWriter = dbChangesWriter ?? throw new ArgumentNullException(nameof(dbChangesWriter));

        if (Connection.State != ConnectionState.Open)
            Connection.Open();
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return Task.CompletedTask;

        _transaction = Connection.BeginTransaction();
        return Task.CompletedTask;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        await _dbChangesWriter.SaveChangesAsync(cancellationToken);

        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;

        return;
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return Task.CompletedTask;

        try
        {
            _transaction.Rollback();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }

        return Task.CompletedTask;
    }

    public Task CreateSavepointAsync(string name, CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Não há transação ativa. Savepoints exigem transação iniciada pelo middleware.");

        ValidateSavepointName(name);
        var sql = GetSavepointCreateSql(name);
        Connection.Execute(sql, transaction: _transaction);
        return Task.CompletedTask;
    }

    public Task RollbackToSavepointAsync(string name, CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Não há transação ativa.");

        ValidateSavepointName(name);
        var sql = GetSavepointRollbackSql(name);
        Connection.Execute(sql, transaction: _transaction);
        return Task.CompletedTask;
    }

    public string Offset() => "OFFSET @PageIndex * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

    private static void ValidateSavepointName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 32)
            throw new ArgumentException("Nome do savepoint deve ter entre 1 e 32 caracteres.", nameof(name));

        foreach (var c in name)
        {
            var isValid = c == '_' ||
                            (c >= 'a' && c <= 'z') ||
                            (c >= 'A' && c <= 'Z') ||
                            (c >= '0' && c <= '9');

            if (!isValid)
                throw new ArgumentException("Nome do savepoint deve conter apenas [a-zA-Z0-9_].", nameof(name));
        }
    }

    private static string GetSavepointCreateSql(string name) => $"SAVE TRANSACTION sp_{name}";

    private static string GetSavepointRollbackSql(string name) => $"ROLLBACK TRANSACTION sp_{name}";

    public void Dispose()
    {
        if (_transaction != null)
        {
            try
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
            catch
            {
            }

            _transaction = null;
        }

        Connection.Dispose();
    }
}
