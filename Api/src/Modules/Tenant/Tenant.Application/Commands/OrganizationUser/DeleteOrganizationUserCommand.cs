using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.OrganizationUser;

public record DeleteOrganizationUserCommand(Guid OrganizationId, Guid Id, byte[] RowVersion) : ICommand<OrganizationUserDto>;

