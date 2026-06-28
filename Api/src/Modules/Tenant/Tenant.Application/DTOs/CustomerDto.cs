using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record CustomerDto : OrganizationInactiveDto
{
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
}