using MediatR;

namespace BuildingBlocks.Application.Interfaces.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
