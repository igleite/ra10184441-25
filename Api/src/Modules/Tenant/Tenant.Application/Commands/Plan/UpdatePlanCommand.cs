using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Plan;

public record UpdatePlanCommand(Guid Id, string Name, string Description, int MaxUsers, int MaxClients, int MaxTickets, byte[] RowVersion) : ICommand<PlanDto>;

