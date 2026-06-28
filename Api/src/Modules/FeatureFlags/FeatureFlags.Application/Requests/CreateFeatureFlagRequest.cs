namespace FeatureFlags.Application.Requests;

public record CreateFeatureFlagRequest(string Name, string Description);