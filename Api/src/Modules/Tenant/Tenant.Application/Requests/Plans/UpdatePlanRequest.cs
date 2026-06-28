namespace Tenant.Application.Requests.Plans;

public record UpdatePlanRequest(string Name, string Description, int MaxUsers, int MaxClients, int MaxTickets, byte[] RowVersion);