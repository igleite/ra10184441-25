using BuildingBlocks.Infrastructure.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Interfaces;
using Tickets.Domain.Extensions;
using Tickets.Infrastructure.Dapper;
using Tickets.Infrastructure.Repositories;

namespace Tickets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        SqlMapper.AddTypeHandler(new StatusTypeHandler());
        SqlMapper.AddTypeHandler(new AllocationCenterHandler());

        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IStatusReasonRepository, StatusReasonRepository>();
        services.AddScoped<ITicketClassificationRepository, TicketClassificationRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }
}

