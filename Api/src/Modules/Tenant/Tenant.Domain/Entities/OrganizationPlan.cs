using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class OrganizationPlan : OrganizationInactiveEntity
{
    public Guid PlanId { get; protected set; } = Guid.Empty;
    public string Description { get; protected set; } = string.Empty;
    public int MaxUsers { get; protected set; } = 0;
    public int MaxClients { get; protected set; } = 0;
    public int MaxTickets { get; protected set; } = 0;

    protected OrganizationPlan() { }

    public OrganizationPlan(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetPlanId(Guid planId, DateTime updatedAt)
    {
        PlanId = planId;
        SetUpdatedAt(updatedAt);
    }

    public void SetMaxUsers(int maxUsers, DateTime updatedAt)
    {
        MaxUsers = maxUsers;
        SetUpdatedAt(updatedAt);
    }

    public void SetMaxClients(int maxClients, DateTime updatedAt)
    {
        MaxClients = maxClients;
        SetUpdatedAt(updatedAt);
    }

    public void SetMaxTickets(int maxTickets, DateTime updatedAt)
    {
        MaxTickets = maxTickets;
        SetUpdatedAt(updatedAt);
    }

    public void SetDescription(string description, DateTime updatedAt)
    {
        Description = description;
        SetUpdatedAt(updatedAt);
    }
}

