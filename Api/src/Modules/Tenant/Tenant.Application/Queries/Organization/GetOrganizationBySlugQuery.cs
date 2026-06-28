using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Organization;

public record GetOrganizationBySlugQuery(string slug) : IQuery<OrganizationDto>;