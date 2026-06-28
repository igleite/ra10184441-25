using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.CustomerUser;

public record CreateCustomerUserCommand(Guid OrganizationId, Guid CustomerId, Guid UserId) : ICommand<CustomerUserDto>;

