using BuildingBlocks.Domain.ValueObjects;

namespace Tickets.Domain.ValueObjects;

public sealed class StatusType : ValueObject
{
    public int Value { get; }

    public static readonly StatusType Open = new(1);
    public static readonly StatusType Closed = new(2);

    private StatusType(int value)
    {
        Value = value;
    }

    public bool IsOpen => this == Open;
    public bool IsClosed => this == Closed;

    public static StatusType From(int value)
    {
        return value switch
        {
            1 => Open,
            2 => Closed,
            _ => throw new Exception("Invalid status type")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
