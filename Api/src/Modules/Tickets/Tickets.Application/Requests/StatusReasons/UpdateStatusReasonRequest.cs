namespace Tickets.Application.Requests.StatusReasons;

public record UpdateStatusReasonRequest(int Type, string Name, byte[] RowVersion);