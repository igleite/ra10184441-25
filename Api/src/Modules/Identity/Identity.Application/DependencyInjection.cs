using BuildingBlocks.Application.Interfaces.Command;
using BuildingBlocks.Application.Results;
using FluentValidation;
using Identity.Application.Commands.MagicLink;
using Identity.Application.Commands.Session;
using Identity.Application.Commands.VerificationToken;
using Identity.Application.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoginRequest>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddCommands();

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<SendMagicLinkCommand>, SendMagicLinkCommandHandler>();
        services.AddScoped<ICommandHandler<ConsumeVerificationTokenCommand, string>, ConsumeVerificationTokenCommandHandler>();
        services.AddScoped<ICommandHandler<CreateSessionCommand>, CreateSessionCommandHandler>();
        services.AddScoped<ICommandHandler<ValidateSessionCommand, Guid>, ValidateSessionCommandHandler>();
        services.AddScoped<ICommandHandler<LogoutSessionCommand>, LogoutSessionCommandHandler>();

        return services;
    }
}
