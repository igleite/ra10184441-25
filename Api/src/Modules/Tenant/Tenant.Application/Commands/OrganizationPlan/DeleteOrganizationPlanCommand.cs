using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.OrganizationPlan;

public record DeleteOrganizationPlanCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<OrganizationPlanDto>;

