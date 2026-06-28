using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Customer;

public record UpdateCustomerCommand(Guid OrganizationId, Guid Id, string Name, string Document, byte[] RowVersion) : ICommand<CustomerDto>;

