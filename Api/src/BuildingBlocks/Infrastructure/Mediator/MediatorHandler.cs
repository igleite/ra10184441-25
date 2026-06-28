using BuildingBlocks.Application.Interfaces.Mediator;
using BuildingBlocks.Application.Interfaces.Query;
using MediatR;

namespace BuildingBlocks.Infrastructure.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;
    
    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
    {
        return await _mediator.Send<TResult>(query, cancellationToken);
    }

    public async Task<TResult> SendCommand<TRequest, TResult>(TRequest command, CancellationToken cancellationToken = default) where TRequest : IRequest<TResult>
    {
        return await _mediator.Send<TResult>(command, cancellationToken);
    }

    public async Task SendCommand<TRequest>(TRequest command, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        await _mediator.Send(command, cancellationToken);
    }
}