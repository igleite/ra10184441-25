using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.OrganizationPlan;

public record UpdateOrganizationPlanCommand(Guid OrganizationId, Guid Id, Guid PlanId, string Description, int MaxUsers, int MaxClients, int MaxTickets, byte[] RowVersion) : ICommand<OrganizationPlanDto>;

