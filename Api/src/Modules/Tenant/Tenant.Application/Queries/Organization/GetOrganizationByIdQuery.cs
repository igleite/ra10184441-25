using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Organization;

public record GetOrganizationByIdQuery(Guid Id) : IQuery<OrganizationDto>;