using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Plan;

public record CreatePlanCommand(string Name, string Description, int MaxUsers, int MaxClients, int MaxTickets) : ICommand<PlanDto>;

