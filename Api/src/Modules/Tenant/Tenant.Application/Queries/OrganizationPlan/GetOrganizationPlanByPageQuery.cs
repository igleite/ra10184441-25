using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationPlan;

public record GetOrganizationPlanByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : PageQuery<OrganizationPlanDto>(PageIndex, PageSize);

