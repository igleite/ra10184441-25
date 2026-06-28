using BuildingBlocks.Application.Interfaces.Query;
using Tenant.Application.DTOs;
using Tenant.Domain.Entities;

namespace Tenant.Application.Queries.Customer;

public record GetCustomerByIdQuery(Guid OrganizationId, Guid Id) : IQuery<CustomerDto>;