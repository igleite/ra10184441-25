using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Command;
using BuildingBlocks.Application.Interfaces.Query;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Commands;
using Tickets.Application.Commands.Chat;
using Tickets.Application.Commands.StatusReason;
using Tickets.Application.Commands.TicketClassification;
using Tickets.Application.Commands.Tickets;
using Tickets.Application.DTOs;
using Tickets.Application.Queries.Chat;
using Tickets.Application.Queries.StatusReason;
using Tickets.Application.Queries.TicketClassification;
using Tickets.Application.Queries.Tickets;
using Tickets.Application.Requests.Tickets;

namespace Tickets.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateTicketRequest>();

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
        services.AddScoped<IQueryHandler<GetStatusReasonByPageQuery, PageDto<StatusReasonDto>>, GetStatusReasonByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetStatusReasonByIdQuery, StatusReasonDto>, GetStatusReasonByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetTicketClassificationByPageQuery, PageDto<TicketClassificationDto>>, GetTicketClassificationByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetTicketClassificationByIdQuery, TicketClassificationDto>, GetTicketClassificationByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetTicketByPageQuery, PageDto<TicketDto>>, GetTicketByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetTicketByIdQuery, TicketDto>, GetTicketByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetChatByPageQuery, PageDto<ChatDto>>, GetChatByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetChatByIdQuery, ChatDto>, GetChatByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetChatByTicketIdQuery, ChatDto>, GetChatByTicketIdQueryHandler>();

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateStatusReasonCommand, StatusReasonDto>, CreateStatusReasonCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateStatusReasonCommand, StatusReasonDto>, UpdateStatusReasonCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteStatusReasonCommand, StatusReasonDto>, DeleteStatusReasonCommandHandler>();

        services.AddScoped<ICommandHandler<CreateTicketClassificationCommand, TicketClassificationDto>, CreateTicketClassificationCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTicketClassificationCommand, TicketClassificationDto>, UpdateTicketClassificationCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTicketClassificationCommand, TicketClassificationDto>, DeleteTicketClassificationCommandHandler>();

        services.AddScoped<ICommandHandler<CreateTicketCommand, TicketDto>, CreateTicketCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTicketCommand, TicketDto>, UpdateTicketCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTicketCommand, TicketDto>, DeleteTicketCommandHandler>();

        services.AddScoped<ICommandHandler<CreateChatCommand, ChatDto>, CreateChatCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateChatCommand, ChatDto>, UpdateChatCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteChatCommand, ChatDto>, DeleteChatCommandHandler>();

        return services;
    }
}

