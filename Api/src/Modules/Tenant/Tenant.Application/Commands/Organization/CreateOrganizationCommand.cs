using BuildingBlocks.Application.Interfaces.Command;
using Tenant.Application.DTOs;

namespace Tenant.Application.Commands.Organization;

public record CreateOrganizationCommand(string Name, string Document, string Slug) : ICommand<OrganizationDto>;

