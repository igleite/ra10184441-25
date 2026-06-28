using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Application.Interfaces;
using Tenant.Application.Interfaces.Resolvers;
using Tenant.Infrastructure.Repositories;
using Tenant.Infrastructure.Resolvers;

namespace Tenant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITenantResolver, HttpTenantResolver>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IOrganizationPlanRepository, OrganizationPlanRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerUserRepository, CustomerUserRepository>();
        services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IArtifactTypeRepository, ArtifactTypeRepository>();
        services.AddScoped<IArtifactRepository, ArtifactRepository>();
        services.AddScoped<ICustomerArtifactRepository, CustomerArtifactRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();

        return services;
    }
}

