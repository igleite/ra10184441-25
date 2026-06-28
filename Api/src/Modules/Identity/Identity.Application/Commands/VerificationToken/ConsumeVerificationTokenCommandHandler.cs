using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Application.Interfaces.Command;
using Identity.Application.Interfaces;

namespace Identity.Application.Commands.VerificationToken;

public class ConsumeVerificationTokenCommandHandler : ICommandHandler<ConsumeVerificationTokenCommand, string>
{
    private readonly IVerificationTokenRepository _verificationTokenRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ConsumeVerificationTokenCommandHandler(
        IVerificationTokenRepository verificationTokenRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _verificationTokenRepository = verificationTokenRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<string> Handle(ConsumeVerificationTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            throw AppException.NotFound("Token inválido ou expirado.");

        var verificationToken = await _verificationTokenRepository.GetByTokenAsync(request.Token);
        if (verificationToken is null || verificationToken.ExpiresAt < _dateTimeProvider.UtcNow)
            throw AppException.NotFound("Token inválido ou expirado.");

        await _verificationTokenRepository.DeleteByTokenAsync(request.Token);

        return verificationToken.Email;
    }
}
