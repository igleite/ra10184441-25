using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record ArtifactDto : InactiveDto
{
    public Guid ArtifactTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
