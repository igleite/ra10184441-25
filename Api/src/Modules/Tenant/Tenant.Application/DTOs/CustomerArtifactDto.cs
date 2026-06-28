using BuildingBlocks.Application.DTOs;

namespace Tenant.Application.DTOs;

public record CustomerArtifactDto : InactiveDto
{
    public Guid CustomerId { get; set; }
    public Guid ArtifactId { get; set; }
}
