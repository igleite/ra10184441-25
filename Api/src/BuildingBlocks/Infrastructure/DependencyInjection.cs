using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Interfaces.Services;
using BuildingBlocks.Application.Interfaces.Uow;
using BuildingBlocks.Application.Options;
using BuildingBlocks.Infrastructure.Mediator;
using BuildingBlocks.Infrastructure.Services;
using BuildingBlocks.Infrastructure.Uow;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabaseConnection(configuration)
            .AddHttpContextAccessor()
            .AddServices(configuration)
            .AddBackgroundServices(configuration)
            .AddMediator();

        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IEmailService, EmailService>();

        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        return services;
    }

    private static IServiceCollection AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var sdpNewConnectionString = configuration.GetConnectionString("SdpNewConnection") ?? throw new InvalidOperationException("Connection string 'SdpNewConnection' not found.");
        var sdpNewChangesWriter = new DefaultChangesWriter();

        services.AddScoped<IUnitOfWork>(serviceProvider =>
        {
            var uow = new UnitOfWork
            {
                #region Sdp

                SdpDpNewIDbConnection = new SqlConnection(sdpNewConnectionString),
                SdpDpNewSqlServerDbChangesWriter = sdpNewChangesWriter,

                #endregion
            };

            return uow;
        });

        return services;
    }
}
