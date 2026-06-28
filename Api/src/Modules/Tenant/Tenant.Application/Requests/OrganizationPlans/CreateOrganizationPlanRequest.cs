namespace Tenant.Application.Requests.OrganizationPlans;

public record CreateOrganizationPlanRequest(Guid PlanId, string Description, int MaxUsers, int MaxClients, int MaxTickets);