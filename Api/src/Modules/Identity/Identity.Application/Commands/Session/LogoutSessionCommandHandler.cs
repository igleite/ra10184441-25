using BuildingBlocks.Application.Interfaces.Command;
using Identity.Application.Interfaces;

namespace Identity.Application.Commands.Session;

public class LogoutSessionCommandHandler : ICommandHandler<LogoutSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;

    public LogoutSessionCommandHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task Handle(LogoutSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByTokenAsync(request.BearerToken);
        if (session is not null)
            await _sessionRepository.DeleteByTokenAsync(request.BearerToken);
    }
}
