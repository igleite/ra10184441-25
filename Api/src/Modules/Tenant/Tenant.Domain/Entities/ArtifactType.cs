using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class ArtifactType : OrganizationInactiveEntity
{
    public string Name { get; protected set; } = string.Empty;

    protected ArtifactType() { }

    public ArtifactType(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetName(string name, DateTime updatedAt)
    {
        Name = name;
        SetUpdatedAt(updatedAt);
    }
}
