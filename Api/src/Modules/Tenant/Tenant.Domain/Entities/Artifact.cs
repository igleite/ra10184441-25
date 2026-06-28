using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class Artifact : InactiveEntity
{
    public Guid ArtifactTypeId { get; protected set; } = Guid.Empty;
    public string Name { get; protected set; } = string.Empty;
    public string Code { get; protected set; } = string.Empty;

    protected Artifact() { }

    public Artifact(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetArtifactTypeId(Guid artifactTypeId, DateTime updatedAt)
    {
        ArtifactTypeId = artifactTypeId;
        SetUpdatedAt(updatedAt);
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
