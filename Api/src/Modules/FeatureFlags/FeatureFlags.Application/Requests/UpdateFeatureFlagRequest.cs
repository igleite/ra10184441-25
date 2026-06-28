namespace FeatureFlags.Application.Requests;

public record UpdateFeatureFlagRequest(string Name, string Description, bool Value, byte[] RowVersion);