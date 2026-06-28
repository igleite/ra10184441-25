using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.OrganizationPlan;

public record CreateOrganizationPlanCommand(Guid OrganizationId, Guid PlanId, string Description, int MaxUsers, int MaxClients, int MaxTickets) : ICommand<OrganizationPlanDto>;

