namespace Tenant.Application.Requests.Plans;

public record CreatePlanRequest(string Name, string Description, int MaxUsers, int MaxClients, int MaxTickets);