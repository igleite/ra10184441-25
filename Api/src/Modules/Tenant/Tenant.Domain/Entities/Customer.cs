using BuildingBlocks.Domain.Entities;

namespace Tenant.Domain.Entities;

public class Customer : OrganizationInactiveEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string Document { get; protected set; } = string.Empty;

    protected Customer() { }

    public Customer(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
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
}