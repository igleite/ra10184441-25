namespace BuildingBlocks.Domain.Entities;

public abstract class InactiveEntity : Entity
{
    public DateTime? InactivedAt { get; protected set; }

    protected InactiveEntity() { }

    protected InactiveEntity(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetInactivedAt(DateTime? deletedAt, DateTime updatedAt)
    {
        InactivedAt = deletedAt;
        SetUpdatedAt(updatedAt);
    }

    public bool IsInactived => InactivedAt.HasValue;
}
