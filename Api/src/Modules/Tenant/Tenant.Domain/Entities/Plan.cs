using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class Plan : InactiveEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Description { get; protected set; } = string.Empty;
    public int MaxUsers { get; protected set; } = 0;
    public int MaxClients { get; protected set; } = 0;
    public int MaxTickets { get; protected set; } = 0;

    protected Plan() { }

    public Plan(Guid id, DateTime createdAt) : base(id, createdAt)
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
}

