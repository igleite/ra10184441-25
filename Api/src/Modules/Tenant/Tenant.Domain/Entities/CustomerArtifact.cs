using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class CustomerArtifact : InactiveEntity
{
    public Guid CustomerId { get; protected set; } = Guid.Empty;
    public Guid ArtifactId { get; protected set; } = Guid.Empty;

    protected CustomerArtifact() { }

    public CustomerArtifact(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetCustomerId(Guid customerId, DateTime updatedAt)
    {
        CustomerId = customerId;
        SetUpdatedAt(updatedAt);
    }

    public void SetArtifactId(Guid artifactId, DateTime updatedAt)
    {
        ArtifactId = artifactId;
        SetUpdatedAt(updatedAt);
    }
}
