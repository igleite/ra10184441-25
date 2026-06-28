using Identity.Application.Interfaces;
using Identity.Application.Options;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.SectionName));
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();

        services.AddHttpClient<IIdentityService, IdentityService>((sp, client) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var url = config["Identity:SdpApiUrl"]!;

            client.BaseAddress = new Uri(url.EndsWith("/") ? url : url + "/");
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("accept-language", "pt-BR");
        });

        return services;
    }
}
