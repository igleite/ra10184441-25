using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;

namespace Tenant.Application.Queries.CustomerUser;

public record GetCustomerUserByIdQuery(Guid OrganizationId, Guid Id) : IQuery<CustomerUserDto>;

