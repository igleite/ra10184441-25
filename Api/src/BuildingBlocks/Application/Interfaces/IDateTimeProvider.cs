namespace BuildingBlocks.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}