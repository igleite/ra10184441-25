using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.OrganizationUser;

public record UpdateOrganizationUserCommand(Guid OrganizationId, Guid Id, Guid UserId, Guid TeamId, byte[] RowVersion) : ICommand<OrganizationUserDto>;

