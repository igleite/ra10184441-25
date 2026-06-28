namespace BuildingBlocks.Domain.Entities;

public abstract class OrganizationInactiveEntity : InactiveEntity
{
    public Guid OrganizationId { get; protected set; } = default!;

    protected OrganizationInactiveEntity() { }

    protected OrganizationInactiveEntity(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt)
    {
        OrganizationId = organizationId;
    }
}
