namespace BuildingBlocks.Application.Helpers;

public static class AppHostHelper
{
    public static readonly HashSet<string> ReservedInfrastructureSubdomains =
        new(StringComparer.OrdinalIgnoreCase) { "api", "www" };

    /// <summary>
    /// Hosts de infraestrutura (ex.: api.localtest.me) — não são tenant nem frontend.
    /// </summary>
    public static bool IsReservedInfrastructureHost(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
            return false;

        var hostname = host.Trim().Split(':')[0];
        var parts = hostname.Split('.');

        if (parts.Length <= 1)
            return false;

        if (parts.Length == 2 && !string.Equals(parts[1], "localhost", StringComparison.OrdinalIgnoreCase))
            return false;

        return ReservedInfrastructureSubdomains.Contains(parts[0]);
    }

    /// <summary>
    /// Extrai slug de tenant do host, ignorando subdomínios reservados e hosts raiz.
    /// </summary>
    public static string? ExtractTenantSlug(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
            return null;

        var hostname = host.Trim().Split(':')[0];
        var parts = hostname.Split('.');

        if (parts.Length <= 1)
            return null;

        if (parts.Length == 2 && !string.Equals(parts[1], "localhost", StringComparison.OrdinalIgnoreCase))
            return null;

        var slug = parts[0];
        if (ReservedInfrastructureSubdomains.Contains(slug))
            return null;

        return slug;
    }
}
