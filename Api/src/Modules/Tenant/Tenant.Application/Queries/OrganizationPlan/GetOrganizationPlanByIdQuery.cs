using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationPlan;

public record GetOrganizationPlanByIdQuery(Guid OrganizationId, Guid Id) : IQuery<OrganizationPlanDto>;

