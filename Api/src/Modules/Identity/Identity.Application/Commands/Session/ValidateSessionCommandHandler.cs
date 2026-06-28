using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Identity.Application.Interfaces;

namespace Identity.Application.Commands.Session;

public class ValidateSessionCommandHandler : ICommandHandler<ValidateSessionCommand, Guid>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ValidateSessionCommandHandler(
        ISessionRepository sessionRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _sessionRepository = sessionRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(ValidateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByTokenAsync(request.BearerToken);
        if (session is null)
            throw AppException.Forbidden("Sessão não encontrada.");

        if (session.ExpiresAt < _dateTimeProvider.UtcNow)
        {
            await _sessionRepository.DeleteByTokenAsync(request.BearerToken);
            throw AppException.Forbidden("Sessão expirada.");
        }

        var now = _dateTimeProvider.UtcNow;
        session.SetLastUsedAt(now, now);
        await _sessionRepository.UpdateAsync(session);

        return session.UserId;
    }
}
