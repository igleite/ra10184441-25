using BuildingBlocks.Application.Interfaces.Uow;

namespace BuildingBlocks.Infrastructure.Uow;

public class DefaultChangesWriter : ISqlServerDbChangesWriter
{
    public DefaultChangesWriter()
    {
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}