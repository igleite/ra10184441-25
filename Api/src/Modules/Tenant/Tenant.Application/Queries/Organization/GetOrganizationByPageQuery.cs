using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Organization;

public record GetOrganizationByPageQuery(int PageIndex, int PageSize) : PageQuery<OrganizationDto>(PageIndex, PageSize);
