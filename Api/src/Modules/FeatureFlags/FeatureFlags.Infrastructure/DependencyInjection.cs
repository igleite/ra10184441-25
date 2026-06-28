using FeatureFlags.Application.Interfaces;
using FeatureFlags.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureFlags.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFeatureFlagRepository, FeatureFlagRepository>();

        return services;
    }
}
