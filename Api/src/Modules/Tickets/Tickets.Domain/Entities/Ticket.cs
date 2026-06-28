using BuildingBlocks.Domain.Entities;
using BuildingBlocks.Domain.Events;
using Tickets.Domain.ValueObjects;

namespace Tickets.Domain.Entities;

public class Ticket : OrganizationSoftDeleteEntity
{
    public Guid CustomerId { get; protected set; } = Guid.Empty;
    public Guid ArtifactId { get; protected set; } = Guid.Empty;
    public Guid StatusId { get; protected set; } = Guid.Empty;
    public Guid ClassificationId { get; protected set; } = Guid.Empty;
    public Guid CreatedByUserId { get; protected set; } = Guid.Empty;
    public AllocationCenter AllocationCenter { get; protected set; } = AllocationCenter.Customer;
    public string Description { get; protected set; } = String.Empty;
    public string Resolution { get; protected set; } = String.Empty;

    protected Ticket() { }

    public Ticket(Guid id, DateTime createdAt, Guid organizationId) : base(id, createdAt, organizationId)
    {
    }

    public void SetCustomerId(Guid id, DateTime updatedAt)
    {
        CustomerId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetArtifactId(Guid id, DateTime updatedAt)
    {
        ArtifactId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetStatusId(Guid id, DateTime updatedAt)
    {
        StatusId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetClassificationId(Guid id, DateTime updatedAt)
    {
        ClassificationId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetAllocationCenter(AllocationCenter allocationCenter, DateTime updatedAt)
    {
        AllocationCenter = allocationCenter;
        SetUpdatedAt(updatedAt);
    }

    public void SetCreatedByUserId(Guid id, DateTime updatedAt)
    {
        CreatedByUserId = id;
        SetUpdatedAt(updatedAt);
    }

    public void SetDescription(string description, DateTime updatedAt)
    {
        Description = description;
        SetUpdatedAt(updatedAt);
    }

    public void SetResolution(string resolution, DateTime updatedAt)
    {
        Resolution = resolution;
        SetUpdatedAt(updatedAt);
    }

    public void RaiseTicketCreatedEvent()
    {
        _domainEvents.Add(new TicketCreatedEvent(
            Id,
            OrganizationId,
            CustomerId,
            StatusId,
            CreatedByUserId,
            Description,
            CreatedAt
        ));
    }
}