using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Plan;

public record DeletePlanCommand(Guid Id, byte[] RowVersion) : ICommand<PlanDto>;

