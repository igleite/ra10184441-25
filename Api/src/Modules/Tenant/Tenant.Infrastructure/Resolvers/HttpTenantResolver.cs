using Microsoft.AspNetCore.Http;
using Tenant.Application.Helpers;
using Tenant.Application.Interfaces.Resolvers;

namespace Tenant.Infrastructure.Resolvers;

public class HttpTenantResolver : ITenantResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTenantResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public TenantResolution ResolveWithSource()
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx is null)
        {
            return new TenantResolution(null, TenantResolutionSource.None, null);
        }

        var requestHost = ctx.Request.Host.Host;
        var slug = TenantHelper.ExtractSlug(requestHost);
        if (slug is not null)
        {
            return new TenantResolution(slug, TenantResolutionSource.RequestHost, requestHost);
        }

        var forwardedHostRaw = GetHeaderValue(ctx, "X-Forwarded-Host");
        var forwardedHost = ExtractHost(forwardedHostRaw);
        slug = TenantHelper.ExtractSlug(forwardedHost);
        if (slug is not null)
        {
            return new TenantResolution(slug, TenantResolutionSource.ForwardedHost, forwardedHost);
        }

        var originRaw = GetHeaderValue(ctx, "Origin");
        var originHost = ExtractHost(originRaw);
        slug = TenantHelper.ExtractSlug(originHost);
        if (slug is not null)
        {
            return new TenantResolution(slug, TenantResolutionSource.Origin, originHost);
        }

        var refererRaw = GetHeaderValue(ctx, "Referer");
        var refererHost = ExtractHost(refererRaw);
        slug = TenantHelper.ExtractSlug(refererHost);
        if (slug is not null)
        {
            return new TenantResolution(slug, TenantResolutionSource.Referer, refererHost);
        }

        return new TenantResolution(null, TenantResolutionSource.None, requestHost);
    }

    private static string? GetHeaderValue(HttpContext ctx, string headerName)
    {
        return ctx.Request.Headers.TryGetValue(headerName, out var values) ? values.ToString() : null;
    }

    private static string? ExtractHost(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;

        var first = raw.Split(',')[0].Trim();

        if (Uri.TryCreate(first, UriKind.Absolute, out var uri))
        {
            return uri.Host;
        }

        if (first.StartsWith('['))
        {
            var end = first.IndexOf(']');
            if (end > 1)
            {
                return first.Substring(1, end - 1);
            }
        }

        var colonIdx = first.IndexOf(':');
        if (colonIdx > 0)
        {
            return first[..colonIdx];
        }

        return first;
    }
}
