using BuildingBlocks.Application.Queries;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.Customer;

public record GetCustomerByPageQuery(Guid OrganizationId, int PageIndex, int PageSize) : PageQuery<CustomerDto>(PageIndex, PageSize);
