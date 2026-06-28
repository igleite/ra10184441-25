using BuildingBlocks.Application.DTOs;

namespace FeatureFlags.Application.DTOs;

public record FeatureFlagDto : EntityDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Value { get; set; }
}
