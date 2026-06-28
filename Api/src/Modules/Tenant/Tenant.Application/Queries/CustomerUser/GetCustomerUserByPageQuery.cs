using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.CustomerUser;

public record GetCustomerUserByPageQuery(Guid OrganizationId, Guid CustomerId, int PageIndex, int PageSize) : PageQuery<CustomerUserDto>(PageIndex, PageSize);

