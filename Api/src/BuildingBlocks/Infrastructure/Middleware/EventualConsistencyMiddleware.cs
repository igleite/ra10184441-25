using BuildingBlocks.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate _next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    public async Task InvokeAsync(HttpContext context, IPublisher publisher)
    {
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsKey, out var value) && value is Queue<IDomainEvent> domainEvents)
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }
            }
            catch (Exception)
            {
            }
        });

        await _next(context);
    }
}
