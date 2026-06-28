using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Command;
using BuildingBlocks.Application.Interfaces.Query;
using FeatureFlags.Application.Commands;
using FeatureFlags.Application.DTOs;
using FeatureFlags.Application.Queries;
using FeatureFlags.Application.Requests;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureFlags.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateFeatureFlagRequest>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services
            .AddQueries()
            .AddCommands();

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetFeatureFlagByPageQuery, PageDto<FeatureFlagDto>>, GetFeatureFlagByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetFeatureFlagByIdQuery, FeatureFlagDto>, GetFeatureFlagByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetFeatureFlagByNameQuery, FeatureFlagDto>, GetFeatureFlagByNameQueryHandler>();

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateFeatureFlagCommand, FeatureFlagDto>, CreateFeatureFlagCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateFeatureFlagCommand, FeatureFlagDto>, UpdateFeatureFlagCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteFeatureFlagCommand, FeatureFlagDto>, DeleteFeatureFlagCommandHandler>();

        return services;
    }
}
