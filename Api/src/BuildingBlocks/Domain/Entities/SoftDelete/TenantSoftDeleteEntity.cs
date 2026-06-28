namespace BuildingBlocks.Domain.Entities;

public abstract class OrganizationSoftDeleteEntity : SoftDeleteEntity
{
    public Guid OrganizationId { get; protected set; } = default!;

    protected OrganizationSoftDeleteEntity() { }

    protected OrganizationSoftDeleteEntity(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt)
    {
        OrganizationId = organizationId;
    }
}

