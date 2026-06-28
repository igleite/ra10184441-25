using MediatR;

namespace BuildingBlocks.Application.Interfaces.Command;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommand : IRequest
{
}