using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Team;

public record GetTeamByIdQuery(Guid OrganizationId, Guid Id) : IQuery<TeamDto>;
