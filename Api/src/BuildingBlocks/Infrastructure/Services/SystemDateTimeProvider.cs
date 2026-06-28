using BuildingBlocks.Application.Interfaces;

namespace BuildingBlocks.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}