namespace BuildingBlocks.Application.Interfaces.Uow;

public interface IUnitOfWork : IDisposable
{
    ISqlServerDbUnitOfWork SdpDpNew { get; }
}