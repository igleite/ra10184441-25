using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Plan;

public record GetPlanByPageQuery(int PageIndex, int PageSize) : PageQuery<PlanDto>(PageIndex, PageSize);

