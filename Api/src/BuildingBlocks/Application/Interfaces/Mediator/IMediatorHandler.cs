using BuildingBlocks.Application.Interfaces.Query;
using MediatR;

namespace BuildingBlocks.Application.Interfaces.Mediator;

public interface IMediatorHandler
{
    Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    Task<TResult> SendCommand<TRequest, TResult>(TRequest command, CancellationToken cancellationToken = default) where TRequest : IRequest<TResult>;
    Task SendCommand<TRequest>(TRequest command, CancellationToken cancellationToken = default) where TRequest : IRequest;
}