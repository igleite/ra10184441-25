namespace Tenant.Application.Requests.OrganizationPlans;

public record UpdateOrganizationPlanRequest(Guid PlanId, string Description, int MaxUsers, int MaxClients, int MaxTickets, byte[] RowVersion);