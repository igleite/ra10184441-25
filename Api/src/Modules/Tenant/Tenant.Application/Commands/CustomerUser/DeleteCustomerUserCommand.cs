using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.CustomerUser;

public record DeleteCustomerUserCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<CustomerUserDto>;

