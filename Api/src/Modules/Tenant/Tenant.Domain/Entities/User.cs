using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class User : InactiveEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
    public Guid? RoleId { get; protected set; }

    protected User() { }

    public User(Guid id, DateTime createdAt) : base(id, createdAt)
    {
    }

    public void SetRoleId(Guid? roleId, DateTime updatedAt)
    {
        RoleId = roleId;
        SetUpdatedAt(updatedAt);
    }

    public void SetName(string name, DateTime updatedAt)
    {
        Name = name;
        SetUpdatedAt(updatedAt);
    }

    public void SetEmail(string email, DateTime updatedAt)
    {
        Email = email;
        SetUpdatedAt(updatedAt);
    }
}

