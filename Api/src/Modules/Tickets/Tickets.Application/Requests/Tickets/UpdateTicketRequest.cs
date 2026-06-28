namespace Tickets.Application.Requests.Tickets;

public record UpdateTicketRequest(Guid StatusId, Guid ClassificationId, Guid ArtifactId, int AllocationCenter, string Description, string Resolution, byte[] RowVersion);