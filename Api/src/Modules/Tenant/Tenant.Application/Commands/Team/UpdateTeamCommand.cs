using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Team;

public record UpdateTeamCommand(Guid OrganizationId, Guid Id, string Name, string Code, Guid RoleId, byte[] RowVersion) : ICommand<TeamDto>;
