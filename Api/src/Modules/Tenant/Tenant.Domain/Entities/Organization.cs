using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class Organization : Entity
{
    public string Name { get; protected set; } = string.Empty;
    public string Document {  get; protected set; } = string.Empty;
    public string Slug {  get; protected set; } = string.Empty;

    protected Organization() { }

    public Organization(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetName(string name, DateTime updatedAt)
    {
        Name = name;
        SetUpdatedAt(updatedAt);
    }

    public void SetDocument(string document, DateTime updatedAt)
    {
        Document = document;
        SetUpdatedAt(updatedAt);
    }

    public void SetSlug(string slug, DateTime updatedAt)
    {
        Slug = slug;
        SetUpdatedAt(updatedAt);
    }
}