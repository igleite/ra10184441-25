using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Team;

public record CreateTeamCommand(Guid OrganizationId, string Name, string Code, Guid RoleId) : ICommand<TeamDto>;
