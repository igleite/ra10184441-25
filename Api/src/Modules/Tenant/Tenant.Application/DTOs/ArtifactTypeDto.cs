using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record ArtifactTypeDto : OrganizationInactiveDto
{
    public string Name { get; set; } = string.Empty;
}
