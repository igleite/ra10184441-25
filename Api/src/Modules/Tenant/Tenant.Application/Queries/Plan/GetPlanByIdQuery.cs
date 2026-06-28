using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Plan;

public record GetPlanByIdQuery(Guid Id) : IQuery<PlanDto>;

