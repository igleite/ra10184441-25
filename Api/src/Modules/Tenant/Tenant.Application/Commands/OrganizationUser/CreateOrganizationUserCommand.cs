using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.OrganizationUser;

public record CreateOrganizationUserCommand(Guid OrganizationId, Guid UserId, Guid TeamId) : ICommand<OrganizationUserDto>;

