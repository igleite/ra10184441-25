namespace Tickets.Application.Requests.Tickets;

public record CreateTicketRequest(Guid CustomerId, Guid ArtifactId, Guid ClassificationId, Guid CreatedByUserId, string Description);