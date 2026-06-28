using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class Team : OrganizationInactiveEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Code { get; protected set; } = string.Empty;
    public Guid RoleId { get; protected set; }

    protected Team() { }

    public Team(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
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

    public void SetRoleId(Guid roleId, DateTime updatedAt)
    {
        RoleId = roleId;
        SetUpdatedAt(updatedAt);
    }
}
