using BuildingBlocks.Application.Helpers;

namespace Tenant.Application.Helpers;

public static class TenantHelper
{
    public static string? ExtractSlug(string? host) =>
        AppHostHelper.ExtractTenantSlug(host);
}
