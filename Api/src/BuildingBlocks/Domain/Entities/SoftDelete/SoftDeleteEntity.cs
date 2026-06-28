namespace BuildingBlocks.Domain.Entities;

public abstract class SoftDeleteEntity : Entity
{
    public DateTime? DeletedAt { get; protected set; }

    protected SoftDeleteEntity() { }

    protected SoftDeleteEntity(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetDeletedAt(DateTime? deletedAt, DateTime updatedAt)
    {
        DeletedAt = deletedAt;
        SetUpdatedAt(updatedAt);
    }

    public bool IsDeleted => DeletedAt.HasValue;
}
