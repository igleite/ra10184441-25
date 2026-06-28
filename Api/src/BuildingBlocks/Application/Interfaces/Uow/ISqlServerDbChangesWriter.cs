namespace BuildingBlocks.Application.Interfaces.Uow;

public interface ISqlServerDbChangesWriter
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}