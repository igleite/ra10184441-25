using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Identity.Application.Interfaces;
using SessionEntity = Identity.Domain.Entities.Session;

namespace Identity.Application.Commands.Session;

public class CreateSessionCommandHandler : ICommandHandler<CreateSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateSessionCommandHandler(
        ISessionRepository sessionRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _sessionRepository = sessionRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var now = _dateTimeProvider.UtcNow;
        var session = new SessionEntity(Guid.NewGuid(), now);
        session.SetUserId(request.UserId, now);
        session.SetToken(request.Token, now);
        session.SetExpiresAt(request.ExpiresAt, now);
        session.SetLastUsedAt(now, now);

        var success = await _sessionRepository.CreateAsync(session);
        if (!success)
            throw AppException.UnprocessableEntity("Não foi possível iniciar a sessão.");
    }
}
