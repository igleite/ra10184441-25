namespace Tenant.Application.Interfaces.Resolvers;

public enum TenantResolutionSource
{
    None = 0,
    RequestHost = 1,
    ForwardedHost = 2,
    Origin = 3,
    Referer = 4
}

