using BuildingBlocks.Application.Interfaces.Uow;
using System.Data;

namespace BuildingBlocks.Infrastructure.Uow;

public sealed class UnitOfWork : IUnitOfWork
{
    private bool _disposed;

    public UnitOfWork()
    {
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        _sdpDpNewConnection?.Dispose();
    }

    #region Sdp

    private ISqlServerDbUnitOfWork _sdpDpNewConnection;
    public IDbConnection SdpDpNewIDbConnection { get; set; }
    public ISqlServerDbChangesWriter SdpDpNewSqlServerDbChangesWriter { get; set; }
    public ISqlServerDbUnitOfWork SdpDpNew => _sdpDpNewConnection ??= new SqlServerDbUnitOfWork(SdpDpNewIDbConnection, SdpDpNewSqlServerDbChangesWriter);

    #endregion

}