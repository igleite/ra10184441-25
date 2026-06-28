using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Customer;

public record CreateCustomerCommand(Guid OrganizationId, string Name, string Document) : ICommand<CustomerDto>;

