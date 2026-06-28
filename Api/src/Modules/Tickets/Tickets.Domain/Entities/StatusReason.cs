using BuildingBlocks.Domain.Entities;
using Tickets.Domain.ValueObjects;

namespace Tickets.Domain.Entities;

public class StatusReason : OrganizationInactiveEntity
{
    public StatusType Type { get; protected set; } = StatusType.Open;
    public string Name { get; protected set; } = String.Empty;
    public bool IsOpeningDefault { get; protected set; } = false;

    protected StatusReason() { }

    public StatusReason(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetType(StatusType type, DateTime updatedAt)
    {
        Type = type;
        SetUpdatedAt(updatedAt);
    }

    public void SetName(string name, DateTime updatedAt)
    {
        Name = name;
        SetUpdatedAt(updatedAt);
    }

    public void SetIsOpeningDefault(bool isOpeningDefault, DateTime updatedAt)
    {
        IsOpeningDefault = isOpeningDefault;
        SetUpdatedAt(updatedAt);
    }
}

