using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Organization;

public record DeleteOrganizationCommand(Guid Id, byte[] RowVersion) : ICommand<OrganizationDto>;

