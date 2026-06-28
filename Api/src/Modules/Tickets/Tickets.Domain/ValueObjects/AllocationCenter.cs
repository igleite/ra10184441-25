using BuildingBlocks.Domain.ValueObjects;

namespace Tickets.Domain.ValueObjects;

public sealed class AllocationCenter : ValueObject
{
    public int Value { get; }

    public static readonly AllocationCenter Customer = new(1);
    public static readonly AllocationCenter Organization = new(2);

    private AllocationCenter(int value)
    {
        Value = value;
    }

    public bool IsCustomer => this == Customer;
    public bool IsOrganization => this == Organization;

    public static AllocationCenter From(int value)
    {
        return value switch
        {
            1 => Customer,
            2 => Organization,
            _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid allocation center")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
