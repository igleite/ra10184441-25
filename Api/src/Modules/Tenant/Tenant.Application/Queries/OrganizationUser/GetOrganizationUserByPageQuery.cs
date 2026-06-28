using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.OrganizationUser;

public record GetOrganizationUserByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : PageQuery<OrganizationUserDto>(PageIndex, PageSize);

