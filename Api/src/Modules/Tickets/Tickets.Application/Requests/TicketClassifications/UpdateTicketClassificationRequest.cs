namespace Tickets.Application.Requests.TicketClassifications;

public record UpdateTicketClassificationRequest(string Name, string Code, byte[] RowVersion);
