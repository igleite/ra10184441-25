using BuildingBlocks.Domain.Events;

namespace BuildingBlocks.Domain.Events;

public record TicketCreatedEvent(
    Guid TicketId,
    Guid OrganizationId,
    Guid CustomerId,
    Guid StatusId,
    Guid CreatedByUserId,
    string Description,
    DateTime CreatedAt
) : IDomainEvent;

