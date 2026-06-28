using Microsoft.Extensions.DependencyInjection;

namespace IA.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
