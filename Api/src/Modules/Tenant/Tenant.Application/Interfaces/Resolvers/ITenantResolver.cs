namespace Tenant.Application.Interfaces.Resolvers;

public interface ITenantResolver
{
    TenantResolution ResolveWithSource();
}