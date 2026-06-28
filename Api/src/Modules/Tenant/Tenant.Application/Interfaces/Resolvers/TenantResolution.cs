namespace Tenant.Application.Interfaces.Resolvers;

public sealed record TenantResolution(string? Slug, TenantResolutionSource Source, string? Host);

