using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Customer;

public record DeleteCustomerCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<CustomerDto>;

