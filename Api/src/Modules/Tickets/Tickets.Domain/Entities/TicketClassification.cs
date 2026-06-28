using BuildingBlocks.Domain.Entities;

namespace Tickets.Domain.Entities;

public class TicketClassification : OrganizationInactiveEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Code { get; protected set; } = string.Empty;

    protected TicketClassification() { }

    public TicketClassification(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetName(string name, DateTime updatedAt)
    {
        Name = name;
        SetUpdatedAt(updatedAt);
    }

    public void SetCode(string code, DateTime updatedAt)
    {
        Code = code;
        SetUpdatedAt(updatedAt);
    }
}
