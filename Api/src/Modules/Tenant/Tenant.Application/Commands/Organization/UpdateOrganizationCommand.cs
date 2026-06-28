using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Organization;

public record UpdateOrganizationCommand(Guid Id, string Name, string Document, string Slug, byte[] RowVersion) : ICommand<OrganizationDto>;

