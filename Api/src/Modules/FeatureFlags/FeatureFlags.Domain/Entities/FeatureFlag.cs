using BuildingBlocks.Domain.Entities;

namespace FeatureFlags.Domain.Entities;

public class FeatureFlag : Entity
{
    public string Name { get; protected set; } = string.Empty;
    public string Description { get; protected set; } = string.Empty;
    public bool Value { get; protected set; } = false;

    protected FeatureFlag() { }

    public FeatureFlag(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetName(string name, DateTime updatedAt)
    {
        Name = name;
        SetUpdatedAt(updatedAt);
    }

    public void SetDescription(string description, DateTime updatedAt)
    {
        Description = description;
        SetUpdatedAt(updatedAt);
    }

    public void SetValue(bool value, DateTime updatedAt)
    {
        Value = value;
        SetUpdatedAt(updatedAt);
    }
}
