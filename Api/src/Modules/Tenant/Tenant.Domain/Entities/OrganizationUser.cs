using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class OrganizationUser : OrganizationInactiveEntity
{
    public Guid UserId { get; protected set; } = Guid.Empty;
    public Guid TeamId { get; protected set; } = Guid.Empty;

    protected OrganizationUser() { }

    public OrganizationUser(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetUserId(Guid userId, DateTime updatedAt)
    {
        UserId = userId;
        SetUpdatedAt(updatedAt);
    }

    public void SetTeamId(Guid teamId, DateTime updatedAt)
    {
        TeamId = teamId;
        SetUpdatedAt(updatedAt);
    }
}

