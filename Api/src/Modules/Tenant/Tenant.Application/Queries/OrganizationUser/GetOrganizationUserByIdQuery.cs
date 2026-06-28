using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationUser;

public record GetOrganizationUserByIdQuery(Guid OrganizationId, Guid Id) : IQuery<OrganizationUserDto>;

