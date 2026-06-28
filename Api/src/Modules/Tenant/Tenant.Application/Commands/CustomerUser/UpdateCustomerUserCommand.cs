using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.CustomerUser;

public record UpdateCustomerUserCommand(Guid OrganizationId, Guid Id, Guid CustomerId, Guid UserId, byte[] RowVersion) : ICommand<CustomerUserDto>;

