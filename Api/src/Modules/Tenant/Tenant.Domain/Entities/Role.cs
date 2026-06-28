namespace Tenant.Domain.Entities;

public class Role
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; } = string.Empty;
    public string Scope { get; protected set; } = string.Empty;
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }

    protected Role() { }

    public Role(Guid id, string name, string scope, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Scope = scope;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
}
