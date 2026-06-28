using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class CustomerUser : OrganizationInactiveEntity
{
    public Guid CustomerId { get; protected set; } = Guid.Empty;
    public Guid UserId { get; protected set; } = Guid.Empty;
    public Guid RoleId { get; protected set; }

    protected CustomerUser() { }

    public CustomerUser(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetRoleId(Guid roleId, DateTime updatedAt)
    {
        RoleId = roleId;
        SetUpdatedAt(updatedAt);
    }

    public void SetCustomerId(Guid customerId, DateTime updatedAt)
    {
        CustomerId = customerId;
        SetUpdatedAt(updatedAt);
    }

    public void SetUserId(Guid userId, DateTime updatedAt)
    {
        UserId = userId;
        SetUpdatedAt(updatedAt);
    }
}

