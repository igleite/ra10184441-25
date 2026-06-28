using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Team;

public record DeleteTeamCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<TeamDto>;
