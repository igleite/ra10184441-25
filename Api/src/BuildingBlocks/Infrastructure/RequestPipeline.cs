using BuildingBlocks.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<TransactionMiddleware>();
        app.UseMiddleware<EventualConsistencyMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}